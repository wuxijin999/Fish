using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;

public class UIAssets
{

    public static GameObject LoadWindow(string name)
    {
        GameObject window = null;
        if (AssetSource.uiFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.UI_WINDOW_PATH, name, ".prefab");
            window = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
        }
        else
        {
            var bundleName = "ui/window";
            var assetInfo = new AssetInfo(bundleName, name);
            window = AssetBundleUtility.Instance.SyncLoadAsset(assetInfo) as GameObject;
        }

        if (window == null)
        {
            DebugEx.LogErrorFormat("UIAssets.LoadWindow() => 加载不到资源: {0}.", name);
        }

        return window;
    }

    public static void LoadWindowAsync(string name, Action<bool, UnityEngine.Object> callBack)
    {
        GameObject window = null;
        if (AssetSource.uiFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.UI_WINDOW_PATH, name, ".prefab");
            window = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (callBack != null)
            {
                callBack(window != null, window);
            }
#endif
        }
        else
        {
            var bundleName = "ui/window";
            var assetInfo = new AssetInfo(bundleName, name);
            AssetBundleUtility.Instance.AsyncLoadAsset(assetInfo, callBack);
        }
    }

    public static GameObject LoadPrefab(string name)
    {
        GameObject prefab = null;
        if (AssetSource.uiFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.UI_PREFAB_PATH, "/", name, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
        }
        else
        {
            var bundleName = "ui/prefab";
            var assetInfo = new AssetInfo(bundleName, name);
            prefab = AssetBundleUtility.Instance.SyncLoadAsset(assetInfo) as GameObject;
        }

        if (prefab == null)
        {
            DebugEx.LogErrorFormat("UIAssets.LoadPrefab() => 加载不到资源: {0}.", name);
        }

        return prefab;
    }

    public static void LoadPrefabAysnc(string name, Action<bool, UnityEngine.Object> callBack)
    {
        GameObject prefab = null;
        if (AssetSource.uiFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.UI_PREFAB_PATH, name, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (callBack != null)
            {
                callBack(prefab != null, prefab);
            }
#endif
        }
        else
        {
            var bundleName = "ui/window";
            var assetInfo = new AssetInfo(bundleName, name);
            AssetBundleUtility.Instance.AsyncLoadAsset(assetInfo, callBack);
        }
    }

    public static void UnLoadWindowAsset(string name)
    {
        if (AssetSource.uiFromEditor)
        {
            AssetBundleUtility.Instance.UnloadAsset("ui/window", name);
        }
    }

    public static void UnLoadPrefabAsset(string name)
    {
        if (AssetSource.uiFromEditor)
        {
            AssetBundleUtility.Instance.UnloadAsset("ui/prefab", name);
        }
    }

    public static Sprite LoadSprite(string folder, string name)
    {
        Sprite sprite = null;
        if (AssetSource.uiFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.UI_SPRITE_ROOT_PATH, "/", folder, "/", name, ".png");
            sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
#endif
        }
        else
        {
            var bundleName = StringUtil.Contact("ui/sprite/", folder);
            var assetInfo = new AssetInfo(bundleName, name);
            AssetBundleUtility.Instance.SyncLoadAsset(assetInfo);
        }

        return sprite;
    }

}
