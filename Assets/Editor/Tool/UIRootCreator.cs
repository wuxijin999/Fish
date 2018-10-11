using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIRootCreator
{

    [MenuItem("GameObject/UIRoot %U", false)]
    public static void CreateUIRoot()
    {
        var uiroot = GameObject.FindObjectOfType<UIRoot>();
        if (uiroot == null)
        {
            var prefab = Resources.Load<GameObject>("UIPrefab/UIRoot");
            var instance = GameObject.Instantiate(prefab);
            instance.name = "UIRoot";
        }

        uiroot = GameObject.FindObjectOfType<UIRoot>();
        var windowRoot = uiroot.transform.GetChildTransformDeeply("WindowRoot");
        if (windowRoot != null)
        {
            Selection.activeObject = windowRoot;
        }

        var uicamera = uiroot.GetComponentInChildren<Camera>(true);
        uicamera.clearFlags = CameraClearFlags.SolidColor;
    }
}
