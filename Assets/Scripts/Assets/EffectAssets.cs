using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectAssets
{

    public static GameObject LoadEffect(int id)
    {
        GameObject prefab = null;
        var config = EffectConfig.Get(id);
        if (AssetSource.effectFromEditor)
        {
#if UNITY_EDITOR
            var path = StringUtil.Contact(AssetPath.EFFECT_ROOT_PATH, "/", config.assetName, ".prefab");
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
        }
        else
        {

        }

        return prefab;
    }

    public static void LoadEffectAsync(int id, Action<bool, UnityEngine.Object> callBack)
    {

    }


}
