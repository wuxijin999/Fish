using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleUtility : SingletonMonobehaviour<AssetBundleUtility>
{
    private List<AssetBundleInfo> m_AssetBundleInfoList = new List<AssetBundleInfo>();
    private Dictionary<string, AssetInfo> m_AssetInfoDict = new Dictionary<string, AssetInfo>();
    private Dictionary<string, AssetBundle> m_AssetBundleDict = new Dictionary<string, AssetBundle>();
    private Dictionary<string, Dictionary<string, UnityEngine.Object>> m_AssetDict = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

    public bool initialized { get; private set; }
    public bool initializedUIAssetBundle { get; private set; }

    public IEnumerator Initialize()
    {
        yield return StartCoroutine(Co_LoadMainfestFile("audio"));
        yield return StartCoroutine(Co_LoadMainfestFile("effect"));
        yield return StartCoroutine(Co_LoadMainfestFile("mob"));
        yield return StartCoroutine(Co_LoadMainfestFile("config"));
        yield return StartCoroutine(Co_LoadMainfestFile("maps"));
        yield return StartCoroutine(Co_LoadMainfestFile("graphic"));
        yield return StartCoroutine(Co_LoadMainfestFile("ui"));

        initialized = true;
    }

    private IEnumerator Co_LoadMainfestFile(string category)
    {
        var path = AssetVersionUtility.GetAssetFilePath(StringUtil.Contact(category, "_assetbundle"));
        var assetBundle = AssetBundle.LoadFromFile(path);

        if (assetBundle == null)
        {
            DebugEx.LogError("AssetBundleManifest的包文件为空或者加载出错.");
            yield break;
        }

        var manifest = assetBundle.LoadAsset<AssetBundleManifest>(AssetPath.AssetDependentFileAssetName);
        if (manifest == null)
        {
            DebugEx.LogError("AssetBundleManifest文件为空或者加载出错.");
            yield break;
        }

        var assetBundleNames = manifest.GetAllAssetBundles();
        foreach (var assetBundleName in assetBundleNames)
        {
            var _dependenices = manifest.GetAllDependencies(assetBundleName);
            var _hash = manifest.GetAssetBundleHash(assetBundleName);
            var assetBundleInfo = new AssetBundleInfo(assetBundleName, _hash, _dependenices);
            m_AssetBundleInfoList.Add(assetBundleInfo);
        }

        assetBundle.Unload(true);
        assetBundle = null;
    }

    public AssetBundleInfo GetAssetBundleInfo(string assetBundleName)
    {
        return m_AssetBundleInfoList.Find((x) => { return x.name == assetBundleName; });
    }

    #region 对AssetBundle资源进行异步协程加载的方法

    public void Co_LoadAsset(AssetInfo assetInfo, Action<bool, UnityEngine.Object> callBack = null)
    {
        Co_LoadAsset(assetInfo.assetBundleName, assetInfo.name, callBack);
    }

    public void Co_LoadAsset(string assetBundleName, string assetName, Action<bool, UnityEngine.Object> callBack = null)
    {
        StartCoroutine(Co_DoLoadAsset(assetBundleName, assetName, callBack));
    }

    private IEnumerator Co_LoadAssetBundle(string assetBundleName)
    {
#if UNITY_5
        assetBundleName = assetBundleName.ToLower();
#endif

        if (JudgeExistAssetBundle(assetBundleName))
        {
            yield break;
        }

        var assetBundleInfo = GetAssetBundleInfo(assetBundleName);
        if (assetBundleInfo == null)
        {
            DebugEx.LogErrorFormat("Co_LoadAssetBundle(): {0}出现错误 => 不存在AssetBundleInfo. ", assetBundleName);
            yield break;
        }

        if (assetBundleInfo.dependentBundles.Length > 0)
        {
            yield return Co_LoadAssetBundleDependenice(assetBundleInfo);
        }

        var filePath = AssetVersionUtility.GetAssetFilePath(assetBundleName);

        DebugEx.LogFormat("Co_LoadAssetBundle(): 将要加载的assetBundle包路径 => {0}", filePath);
        var assetBundle = AssetBundle.LoadFromFile(filePath);
        CacheAssetBundle(assetBundleName, assetBundle);
    }

    private IEnumerator Co_LoadAssetBundleDependenice(AssetBundleInfo assetBundleInfo)
    {
        AssetBundle assetBundle = null;

        if (assetBundleInfo.dependentBundles == null
         || assetBundleInfo.dependentBundles.Length == 0)
        {
            yield break;
        }

        for (int i = 0; i < assetBundleInfo.dependentBundles.Length; ++i)
        {

            if (m_AssetBundleDict.TryGetValue(assetBundleInfo.dependentBundles[i], out assetBundle) == false)
            {
                yield return Co_LoadAssetBundle(assetBundleInfo.dependentBundles[i]);
            }
            else
            {
                if (assetBundle == null)
                {
                    yield return Co_LoadAssetBundle(assetBundleInfo.dependentBundles[i]);
                }
            }
        }
    }

    private IEnumerator Co_DoLoadAsset(string assetBundleName, string assetName, Action<bool, UnityEngine.Object> callBack = null)
    {

        if (JudgeExistAsset(assetBundleName, assetName))
        {
            if (callBack != null)
            {
                callBack(true, m_AssetDict[assetBundleName][assetName]);
            }
            yield break;
        }

        yield return Co_LoadAssetBundle(assetBundleName);

        var request = m_AssetBundleDict[assetBundleName].LoadAssetAsync(assetName);
        while (!request.isDone)
        {
            yield return null;
        }

        if (request.asset != null)
        {
            CacheAsset(assetBundleName, assetName, request.asset);
            if (callBack != null)
            {
                callBack(true, m_AssetDict[assetBundleName][assetName]);
            }
        }
        else
        {
            if (callBack != null)
            {
                callBack(false, null);
            }
        }

    }

    private IEnumerator Co_DoLoadAsset(AssetInfo assetInfo, Action<bool, UnityEngine.Object> callBack = null)
    {
        if (assetInfo == null)
        {
            DebugEx.LogErrorFormat("Co_DoLoadAsset(): {0}, 出现错误 => 存入的AssetInfo为null. ", assetInfo);
            yield break;
        }
        yield return Co_DoLoadAsset(assetInfo.assetBundleName, assetInfo.name, callBack);
    }

    #endregion

    #region 对AssetBundle资源进行同步加载的方法

    public void Sync_LoadAll(string assetBundleName)
    {
        if (JudgeExistAssetBundle(assetBundleName))
        {
            return;
        }

#if UNITY_5
        assetBundleName = assetBundleName.ToLower();
#endif

        Sync_LoadAssetBundle(assetBundleName);

    }

    public UnityEngine.Object Sync_LoadAsset(AssetInfo assetInfo, Type type = null)
    {
        return Sync_LoadAsset(assetInfo.assetBundleName, assetInfo.name, type);
    }

    public UnityEngine.Object Sync_LoadAsset(string assetBundleName, string assetName, Type type = null)
    {

#if UNITY_5
        assetBundleName = assetBundleName.ToLower();
#endif

        UnityEngine.Object @object = null;

        if (JudgeExistAsset(assetBundleName, assetName))
        {

            @object = m_AssetDict[assetBundleName][assetName];

        }
        else
        {

            Sync_LoadAssetBundle(assetBundleName);
            if (m_AssetBundleDict.ContainsKey(assetBundleName))
            {
                if (type != null)
                {
                    @object = m_AssetBundleDict[assetBundleName].LoadAsset(assetName, type);
                }
                else
                {
                    @object = m_AssetBundleDict[assetBundleName].LoadAsset(assetName);
                }

                if (@object != null)
                {
                    CacheAsset(assetBundleName, assetName, @object);
                }
            }

        }

        if (@object == null)
        {
            DebugEx.LogErrorFormat("Sync_LoadAsset(): {0} 出现错误 => null. ", assetName);
        }

        return @object;
    }

    private void Sync_LoadAssetBundle(string assetBundleName)
    {
        if (JudgeExistAssetBundle(assetBundleName))
        {
            return;
        }

        AssetBundleInfo assetBundleInfo = GetAssetBundleInfo(assetBundleName);
        if (assetBundleInfo == null)
        {
            DebugEx.LogErrorFormat("Sync_LoadAssetBundle(): {0} 出现错误 => 不存在AssetBundleInfo. ", assetBundleName);
            return;
        }

        Sync_LoadAssetBundleDependenice(assetBundleInfo);

        string path = AssetVersionUtility.GetAssetFilePath(assetBundleName);
        AssetBundle _assetBundle = AssetBundle.LoadFromFile(path);

        CacheAssetBundle(assetBundleName, _assetBundle);
    }

    private void Sync_LoadAssetBundleDependenice(AssetBundleInfo assetBundleInfo)
    {
        if (assetBundleInfo.dependentBundles == null
         || assetBundleInfo.dependentBundles.Length == 0)
        {
            return;
        }

        AssetBundle assetBundle = null;

        for (int i = 0; i < assetBundleInfo.dependentBundles.Length; ++i)
        {
            if (m_AssetBundleDict.TryGetValue(assetBundleInfo.dependentBundles[i], out assetBundle) == false)
            {
                Sync_LoadAssetBundle(assetBundleInfo.dependentBundles[i]);
            }
            else
            {
                if (assetBundle == null)
                {
                    Sync_LoadAssetBundle(assetBundleInfo.dependentBundles[i]);
                }
            }
        }
    }

    #endregion

    #region 对AssetBundle资源进行异步线程加载的方法
    #endregion

    #region AssetBundle资源卸载方法

    public void UnloadAssetBundle(string assetBundleName, bool unloadAllLoadedObjects, bool includeDependenice)
    {

#if UNITY_5
        assetBundleName = assetBundleName.ToLower();
#endif

        if (JudgeExistAssetBundle(assetBundleName) == false)
        {
            DebugEx.LogWarningFormat("UnloadAssetBundle(): 要卸载的包不在缓存中或者已经被卸载 => {0}. ", assetBundleName);
            return;
        }

        AssetBundleInfo assetBundleInfo = GetAssetBundleInfo(assetBundleName);

        UnloadAssetBundle(assetBundleInfo, unloadAllLoadedObjects, includeDependenice);
    }

    public void UnloadAsset(string assetBundleName, string assetName)
    {
        string assembleName = StringUtil.Contact(assetBundleName, "@", assetName);

        if (JudgeExistAsset(assetBundleName, assetName) == false)
        {
            DebugEx.LogWarningFormat("UnloadAsset(): 要卸载的资源不在缓存中 => {0}. ", assembleName);
            return;
        }

        UnityEngine.Object assetObject = m_AssetDict[assetBundleName][assetName];

        m_AssetDict[assetBundleName].Remove(assetName);

        if (assetObject is GameObject)
        {
            DebugEx.LogFormat("UnloadAsset(): 成功卸载asset资源 => {0}. 类型为{1}, 不做其他处理. ", assetName, assetObject.GetType().Name);
        }
        else
        {
            Resources.UnloadAsset(assetObject);
            DebugEx.LogFormat("UnloadAsset(): 成功卸载asset资源 => {0}. 类型为{1}, 执行Resources.UnloadAsset(). ", assetName, assetObject.GetType().Name);
        }

        if (Application.isEditor)
        {
            Transform asset = transform.Find(assetBundleName + "/Asset:" + assetName);
            if (asset)
            {
                Destroy(asset.gameObject);
            }
        }
    }

    public void UnloadAsset(AssetInfo assetInfo)
    {
        UnloadAsset(assetInfo.assetBundleName, assetInfo.name);
    }

    public void UnloadAll()
    {

        List<string> assetBundleNameList = new List<string>(m_AssetBundleDict.Keys);

        foreach (var assetBundleName in assetBundleNameList)
        {
            UnloadAssetBundle(assetBundleName, true, true);
        }
    }

    private void UnloadAssetBundle(AssetBundleInfo assetBundleInfo, bool unloadAllLoadedObjects, bool includeDependenice)
    {

        // 是否卸载依赖资源
        if (includeDependenice)
        {
            if (assetBundleInfo.dependentBundles != null
             && assetBundleInfo.dependentBundles.Length > 0)
            {

                for (int i = 0; i < assetBundleInfo.dependentBundles.Length; ++i)
                {
                    UnloadAssetBundle(assetBundleInfo.dependentBundles[i], unloadAllLoadedObjects, includeDependenice);
                }

            }
        }

        if (m_AssetDict.ContainsKey(assetBundleInfo.name))
        {

            List<string> _assetNames = new List<string>(m_AssetDict[assetBundleInfo.name].Keys);

            // 卸载对应缓存住的asset资源
            foreach (var _assetName in _assetNames)
            {
                UnloadAsset(assetBundleInfo.name, _assetName);
            }

        }
        else
        {
            DebugEx.LogFormat("UnloadAssetBundle(): 要卸载assetBundle包 {0} 没有资源被加载. ", assetBundleInfo.name);
        }

        // assetBundle包卸载
        m_AssetBundleDict[assetBundleInfo.name].Unload(unloadAllLoadedObjects);

        m_AssetDict.Remove(assetBundleInfo.name);

        // 卸载缓存的assetBundle资源
        m_AssetBundleDict.Remove(assetBundleInfo.name);

        DebugEx.LogFormat("UnloadAssetBundle(): 成功卸载assetBundle包 => {0}. ", assetBundleInfo.name);

        if (Application.isEditor)
        {

            Transform asset = transform.Find(assetBundleInfo.name);
            Transform parent = asset.parent;

            if (asset)
            {
                DestroyImmediate(asset.gameObject);
            }

            if (parent.childCount == 0 && parent != transform)
            {
                DestroyImmediate(parent.gameObject);
            }
        }
    }

    #endregion

    public bool JudgeExistAsset(string assetBundleName, string assetName)
    {
        if (string.IsNullOrEmpty(assetBundleName)
         || string.IsNullOrEmpty(assetName))
        {
            return false;
        }

        if (JudgeExistAssetBundle(assetBundleName) == false)
        {
            return false;
        }

        if (m_AssetDict.ContainsKey(assetBundleName) == false
         || m_AssetDict[assetBundleName].ContainsKey(assetName) == false
         || m_AssetDict[assetBundleName][assetName] == null)
        {
            return false;
        }

        return true;
    }

    public bool JudgeExistAssetBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();

        if (m_AssetBundleDict.ContainsKey(assetBundleName) == false)
        {
            return false;
        }

        if (m_AssetBundleDict[assetBundleName] == null)
        {
            return false;
        }

        return true;
    }

    private void CacheAsset(string assetBundleName, string assetName, UnityEngine.Object asset)
    {
        if (asset == null)
        {
            return;
        }

        if (asset is GameObject)
        {
            (asset as GameObject).SetActive(true);
        }

        if (m_AssetDict.ContainsKey(assetBundleName) == false)
        {
            Dictionary<string, UnityEngine.Object> _temp = new Dictionary<string, UnityEngine.Object>();
            m_AssetDict.Add(assetBundleName, _temp);
        }

        m_AssetDict[assetBundleName][assetName] = asset;

        string assembleName = StringUtil.Contact(assetBundleName, "@", assetName);
        if (m_AssetInfoDict.ContainsKey(assembleName) == false)
        {
            AssetInfo _assetInfo = new AssetInfo(assetBundleName, assetName);
            m_AssetInfoDict[assembleName] = _assetInfo;
        }

        DebugEx.LogFormat("CacheAsset(): 成功缓存asset => {0}. ", assetName);
    }

    private void CacheAssetBundle(string assetBundleName, AssetBundle assetBundle)
    {

        if (assetBundle == null)
        {
            DebugEx.LogErrorFormat("CacheAssetBundle(): {0}出现错误 => 将要缓存的包为 null. ", assetBundleName);
            return;
        }

        if (m_AssetBundleDict.ContainsKey(assetBundleName))
        {
            DebugEx.LogErrorFormat("CacheAssetBundle(): {0}出现错误 => 将要缓存的包已经在缓存中. ", assetBundleName);
            return;
        }

        m_AssetBundleDict[assetBundleName] = assetBundle;
        if (Application.isEditor)
        {
            string[] names = assetBundleName.Split('/');
            string selfPath = string.Empty;
            string parentPath = string.Empty;
            for (int i = 0; i < names.Length; ++i)
            {
                selfPath = names[0];
                for (int j = 1; j <= i; ++j)
                {
                    selfPath = selfPath + "/" + names[j];
                }
                if (transform.Find(selfPath))
                {
                    continue;
                }
                GameObject _go = new GameObject(names[i]);
                if (i == 0)
                {
                    _go.transform.parent = transform;
                }
                else
                {
                    parentPath = names[0];
                    for (int j = 1; j < i; ++j)
                    {
                        parentPath = parentPath + "/" + names[j];
                    }
                    Transform parent = transform.Find(parentPath);
                    if (parent)
                    {
                        _go.transform.parent = parent;
                    }
                }
            }
        }

        DebugEx.LogFormat("CacheAssetBundle(): 成功缓存assetBundle包资源 => {0}. 目前有 {1} 个资源.", assetBundleName, m_AssetBundleDict.Count);
    }

}
