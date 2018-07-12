using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class UILoader
{

    public static GameObject LoadWindow(string _name)
    {

        return null;
    }

    public static void LoadWindowAsync(string _name, UnityAction<bool, UnityEngine.Object> _callBack)
    {
    }

    public static GameObject LoadPrefab(string _name)
    {
        GameObject prefab = null;
        if (AssetSource.uiFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtility.Contact(AssetPath.UI_PREFAB_PATH, Path.DirectorySeparatorChar, _name, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
        }
        else
        {

        }

        return prefab;
    }

    public static void UnLoadWindowAsset(string _name)
    {

    }

}
