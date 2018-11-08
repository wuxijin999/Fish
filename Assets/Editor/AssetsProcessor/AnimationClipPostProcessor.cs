using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationClipPostProcessor : AssetPostprocessor
{

    void OnPreprocessAnimation()
    {
        var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(this.assetPath);
        if (animationClip != null)
        {

        }
    }

}
