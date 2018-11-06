using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

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

    public static void LoadWindowAsync(string name, UnityAction<bool, UnityEngine.Object> callBack)
    {
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

        }

        return prefab;
    }

    public static void UnLoadWindowAsset(string name)
    {

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

        }

        return sprite;
    }

}
