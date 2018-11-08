using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuiltInAssets
{

    public static GameObject LoadPrefab(string name)
    {
        GameObject prefab = null;
        if (AssetSource.builtInFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.BUILTIN_ROOT_PATH, "UIPrefabs/", name, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
        }
        else
        {
            prefab = AssetBundleUtility.Instance.SyncLoadAsset("builtin/prefabs", name) as GameObject;
        }

        if (prefab == null)
        {
            DebugEx.LogErrorFormat("BuiltInAssets.LoadPrefab() => 加载不到资源: {0}.", name);
        }

        return prefab;
    }

    public static void AsyncLoadPrefab(string name, Action<bool, UnityEngine.Object> callBack)
    {
        if (AssetSource.builtInFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.BUILTIN_ROOT_PATH, "UIPrefabs/", name, ".prefab");
            var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (callBack != null)
            {
                callBack(prefab != null, prefab);
            }

            if (prefab == null)
            {
                DebugEx.LogErrorFormat("BuiltInAssets.LoadPrefab() => 加载不到资源: {0}.", name);
            }
#endif
        }
        else
        {
            AssetBundleUtility.Instance.AsyncLoadAsset("builtin/prefabs", name, callBack);
        }
    }

    public static Material LoadMaterial(string name)
    {
        Material material = null;
        if (AssetSource.builtInFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.BUILTIN_ROOT_PATH, "Materials/", name);
            material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(path);
#endif
        }
        else
        {
            material = AssetBundleUtility.Instance.SyncLoadAsset("builtin/materials", name) as Material;
        }

        if (material == null)
        {
            DebugEx.LogErrorFormat("BuiltInAssets.LoadMaterial() => 加载不到资源: {0}.", name);
        }

        return material;
    }

    public static T LoadConfig<T>(string name) where T : ScriptableObject
    {
        T config = null;
        if (AssetSource.builtInFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.BUILTIN_ROOT_PATH, name, "Configs/", ".asset");
            config = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#endif
        }
        else
        {
            config = AssetBundleUtility.Instance.SyncLoadAsset("builtin/configs", name) as T;
        }

        if (config == null)
        {
            DebugEx.LogErrorFormat("BuiltInAssets.LoadConfig() => 加载不到资源: {0}.", name);
        }

        return config;
    }

}
