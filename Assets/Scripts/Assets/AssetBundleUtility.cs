using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleUtility : SingletonMonobehaviour<AssetBundleUtility>
{
    List<AssetBundleInfo> m_AssetBundleInfoList = new List<AssetBundleInfo>();
    Dictionary<string, AssetInfo> m_AssetInfoDict = new Dictionary<string, AssetInfo>();
    Dictionary<string, AssetBundle> m_AssetBundleDict = new Dictionary<string, AssetBundle>();
    Dictionary<string, Dictionary<string, UnityEngine.Object>> m_AssetDict = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

    public void Initialize()
    {
        LoadMainfestFile("audio");
        LoadMainfestFile("effect");
        LoadMainfestFile("mob");
        LoadMainfestFile("config");
        LoadMainfestFile("maps");
        LoadMainfestFile("graphic");
        LoadMainfestFile("ui");
    }

    private void LoadMainfestFile(string category)
    {
        var path = AssetVersionUtility.GetAssetFilePath(StringUtil.Contact(category, "_assetbundle"));
        var assetBundle = AssetBundle.LoadFromFile(path);

        if (assetBundle == null)
        {
            DebugEx.LogError("AssetBundleManifest的包文件为空或者加载出错.");
            return;
        }

        var manifest = assetBundle.LoadAsset<AssetBundleManifest>(AssetPath.AssetDependentFileAssetName);
        if (manifest == null)
        {
            DebugEx.LogError("AssetBundleManifest文件为空或者加载出错.");
            return;
        }

        var assetBundleNames = manifest.GetAllAssetBundles();
        foreach (var assetBundleName in assetBundleNames)
        {
            var dependenices = manifest.GetAllDependencies(assetBundleName);
            var hash = manifest.GetAssetBundleHash(assetBundleName);
            var assetBundleInfo = new AssetBundleInfo(assetBundleName, hash, dependenices);
            this.m_AssetBundleInfoList.Add(assetBundleInfo);
        }

        assetBundle.Unload(true);
        assetBundle = null;
    }

    public AssetBundleInfo GetAssetBundleInfo(string assetBundleName)
    {
        return this.m_AssetBundleInfoList.Find((x) => { return x.name == assetBundleName; });
    }

    public void AsyncLoadAsset(string assetBundleName, string assetName, Action<bool, UnityEngine.Object> callBack = null)
    {
        if (ExistAsset(assetBundleName, assetName))
        {
            if (callBack != null)
            {
                callBack(true, this.m_AssetDict[assetBundleName][assetName]);
            }
        }
        else
        {
            StartCoroutine(Co_LoadAsset(assetBundleName, assetName, callBack));
        }
    }

    private IEnumerator Co_LoadAsset(string assetBundleName, string assetName, Action<bool, UnityEngine.Object> callBack = null)
    {
        SyncLoadAssetBundle(assetBundleName);
        var request = this.m_AssetBundleDict[assetBundleName].LoadAssetAsync(assetName);
        while (!request.isDone)
        {
            yield return null;
        }

        if (request.asset != null)
        {
            CacheAsset(assetBundleName, assetName, request.asset);
            if (callBack != null)
            {
                callBack(true, this.m_AssetDict[assetBundleName][assetName]);
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

    public UnityEngine.Object[] SyncLoadAllAssets(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        SyncLoadAssetBundle(assetBundleName);
        if (ExistAssetBundle(assetBundleName))
        {
            var bundle = m_AssetBundleDict[assetBundleName];
            return bundle.LoadAllAssets();
        }
        else
        {
            return null;
        }
    }

    public UnityEngine.Object SyncLoadAsset(string assetBundleName, string assetName, Type type = null)
    {
        assetBundleName = assetBundleName.ToLower();
        UnityEngine.Object @object = null;
        if (ExistAsset(assetBundleName, assetName))
        {
            @object = this.m_AssetDict[assetBundleName][assetName];
        }
        else
        {
            SyncLoadAssetBundle(assetBundleName);
            if (this.m_AssetBundleDict.ContainsKey(assetBundleName))
            {
                if (type != null)
                {
                    @object = this.m_AssetBundleDict[assetBundleName].LoadAsset(assetName, type);
                }
                else
                {
                    @object = this.m_AssetBundleDict[assetBundleName].LoadAsset(assetName);
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

    private void SyncLoadAssetBundle(string assetBundleName)
    {
        if (ExistAssetBundle(assetBundleName))
        {
            return;
        }

        var assetBundleInfo = GetAssetBundleInfo(assetBundleName);
        LoadAssetBundleDependenice(assetBundleInfo.dependentBundles);

        var path = AssetVersionUtility.GetAssetFilePath(assetBundleName);
        var assetBundle = AssetBundle.LoadFromFile(path);
        CacheAssetBundle(assetBundleName, assetBundle);
    }

    private void LoadAssetBundleDependenice(string[] dependentBundles)
    {
        if (dependentBundles == null
         || dependentBundles.Length == 0)
        {
            return;
        }

        AssetBundle assetBundle = null;
        for (int i = 0; i < dependentBundles.Length; ++i)
        {
            var name = dependentBundles[i];
            if (this.m_AssetBundleDict.TryGetValue(name, out assetBundle))
            {
                if (assetBundle == null)
                {
                    SyncLoadAssetBundle(name);
                }
            }
            else
            {
                SyncLoadAssetBundle(name);
            }
        }
    }

    public void UnloadAssetBundle(string assetBundleName, bool unloadAllLoadedObjects, bool includeDependenice)
    {
        assetBundleName = assetBundleName.ToLower();
        if (!ExistAssetBundle(assetBundleName))
        {
            DebugEx.LogWarningFormat("UnloadAssetBundle(): 要卸载的包不在缓存中或者已经被卸载 => {0}. ", assetBundleName);
            return;
        }

        var assetBundleInfo = GetAssetBundleInfo(assetBundleName);
        UnloadAssetBundle(assetBundleInfo, unloadAllLoadedObjects, includeDependenice);
    }

    private void UnloadAssetBundle(AssetBundleInfo assetBundleInfo, bool unloadAllLoadedObjects, bool includeDependenice)
    {
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

        if (this.m_AssetDict.ContainsKey(assetBundleInfo.name))
        {
            var assetNames = new List<string>(this.m_AssetDict[assetBundleInfo.name].Keys);
            foreach (var assetName in assetNames)
            {
                UnloadAsset(assetBundleInfo.name, assetName);
            }
        }
        else
        {
            DebugEx.LogFormat("UnloadAssetBundle(): 要卸载assetBundle包 {0} 没有资源被加载. ", assetBundleInfo.name);
        }

        this.m_AssetBundleDict[assetBundleInfo.name].Unload(unloadAllLoadedObjects);
        this.m_AssetDict.Remove(assetBundleInfo.name);
        this.m_AssetBundleDict.Remove(assetBundleInfo.name);

        DebugEx.LogFormat("UnloadAssetBundle(): 成功卸载assetBundle包 => {0}. ", assetBundleInfo.name);
    }

    public void UnloadAsset(string assetBundleName, string assetName)
    {
        if (!ExistAsset(assetBundleName, assetName))
        {
            DebugEx.LogWarningFormat("UnloadAsset(): 要卸载的资源不在缓存中 =>bundle: {0}--asset:{1} ", assetBundleName, assetName);
            return;
        }

        var assetObject = this.m_AssetDict[assetBundleName][assetName];
        this.m_AssetDict[assetBundleName].Remove(assetName);

        if (assetObject is GameObject)
        {
            DebugEx.LogFormat("UnloadAsset(): 成功卸载asset资源 => {0}. 类型为gameObject, 不做其他处理. ", assetName);
        }
        else
        {
            Resources.UnloadAsset(assetObject);
            DebugEx.LogFormat("UnloadAsset(): 成功卸载asset资源 => {0}. 类型为{1}, 执行Resources.UnloadAsset(). ", assetName, assetObject.GetType().Name);
        }
    }

    public void UnloadAll()
    {
        var assetBundleNameList = new List<string>(this.m_AssetBundleDict.Keys);
        foreach (var assetBundleName in assetBundleNameList)
        {
            UnloadAssetBundle(assetBundleName, true, true);
        }
    }

    public bool ExistAsset(string assetBundleName, string assetName)
    {
        if (string.IsNullOrEmpty(assetBundleName)
         || string.IsNullOrEmpty(assetName))
        {
            return false;
        }

        if (ExistAssetBundle(assetBundleName) == false)
        {
            return false;
        }

        if (this.m_AssetDict.ContainsKey(assetBundleName) == false
         || this.m_AssetDict[assetBundleName].ContainsKey(assetName) == false
         || this.m_AssetDict[assetBundleName][assetName] == null)
        {
            return false;
        }

        return true;
    }

    public bool ExistAssetBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        if (!this.m_AssetBundleDict.ContainsKey(assetBundleName))
        {
            return false;
        }

        if (this.m_AssetBundleDict[assetBundleName] == null)
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

        if (!this.m_AssetDict.ContainsKey(assetBundleName))
        {
            this.m_AssetDict[assetBundleName] = new Dictionary<string, UnityEngine.Object>();
        }

        this.m_AssetDict[assetBundleName][assetName] = asset;

        var assembleName = StringUtil.Contact(assetBundleName, "@", assetName);
        if (this.m_AssetInfoDict.ContainsKey(assembleName) == false)
        {
            var assetInfo = new AssetInfo(assetBundleName, assetName);
            this.m_AssetInfoDict[assembleName] = assetInfo;
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

        if (this.m_AssetBundleDict.ContainsKey(assetBundleName))
        {
            DebugEx.LogErrorFormat("CacheAssetBundle(): {0}出现错误 => 将要缓存的包已经在缓存中. ", assetBundleName);
            return;
        }

        this.m_AssetBundleDict[assetBundleName] = assetBundle;
        DebugEx.LogFormat("成功缓存assetBundle包资源 => {0}. 目前有 {1} 个资源.", assetBundleName, this.m_AssetBundleDict.Count);
    }

}
