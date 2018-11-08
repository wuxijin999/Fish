using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MobAssets
{

    public static GameObject LoadPrefab(int id)
    {
        GameObject prefab = null;
        var config = MobAssetConfig.Get(id);
        if (AssetSource.mobFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.MOB_ROOT_PATH, config.package, "/", config.assetName, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
        }
        else
        {
            var bundleName = StringUtil.Contact("mob/", config.package);
            prefab = AssetBundleUtility.Instance.SyncLoadAsset(bundleName, config.assetName) as GameObject;
        }

        return prefab;
    }

    public static void LoadPrefabAsync(int id, Action<bool, UnityEngine.Object> callBack)
    {
        GameObject prefab = null;
        var config = MobAssetConfig.Get(id);
        if (AssetSource.mobFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.MOB_ROOT_PATH, config.package, "/", config.assetName, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (callBack != null)
            {
                callBack(prefab != null, prefab);
            }
#endif
        }
        else
        {
            var bundleName = StringUtil.Contact("mob/", config.package);
            AssetBundleUtility.Instance.AsyncLoadAsset(bundleName, config.assetName, callBack);
        }
    }


}
