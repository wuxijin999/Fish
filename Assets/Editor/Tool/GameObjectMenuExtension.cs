using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameObjectMenuExtension
{
    [MenuItem("GameObject/UICustom/Button &1")]
    static void CreateButton()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Prefabs/Button.prefab");
        var instance = GameObject.Instantiate(prefab);
        instance.name = "Button";
        if (Selection.activeTransform != null)
        {
            instance.transform.SetParentEx(Selection.activeTransform)
                                          .SetLocalPosition(Vector3.zero)
                                          .SetLocalEulerAngles(Vector3.zero)
                                          .SetScale(Vector3.one);
        }
    }

    [MenuItem("GameObject/UICustom/ButtonEx &2")]
    static void CreateButtonEx()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Prefabs/ButtonEx.prefab");
        var instance = GameObject.Instantiate(prefab);
        instance.name = "ButtonEx";
        if (Selection.activeTransform != null)
        {
            instance.transform.SetParentEx(Selection.activeTransform)
                                          .SetLocalPosition(Vector3.zero)
                                          .SetLocalEulerAngles(Vector3.zero)
                                          .SetScale(Vector3.one);
        }
    }

    [MenuItem("GameObject/UICustom/Container &3")]
    static void CreateContainer()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Prefabs/Container.prefab");
        var instance = GameObject.Instantiate(prefab);
        instance.name = "Container";
        if (Selection.activeTransform != null)
        {
            (instance.transform as RectTransform).MatchWhith(Selection.activeTransform as RectTransform);
        }
    }

    [MenuItem("GameObject/UICustom/ImageEx &4")]
    static void CreateImageEx()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Prefabs/ImageEx.prefab");
        var instance = GameObject.Instantiate(prefab);
        instance.name = "ImageEx";
        if (Selection.activeTransform != null)
        {
            instance.transform.SetParentEx(Selection.activeTransform)
                                          .SetLocalPosition(Vector3.zero)
                                          .SetLocalEulerAngles(Vector3.zero)
                                          .SetScale(Vector3.one);
        }
    }

    [MenuItem("GameObject/UICustom/TextEx &5")]
    static void CreateTextEx()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Prefabs/TextEx.prefab");
        var instance = GameObject.Instantiate(prefab);
        instance.name = "TextEx";
        if (Selection.activeTransform != null)
        {
            instance.transform.SetParentEx(Selection.activeTransform)
                                          .SetLocalPosition(Vector3.zero)
                                          .SetLocalEulerAngles(Vector3.zero)
                                          .SetScale(Vector3.one);
        }
    }

    [MenuItem("GameObject/UICustom/PatternWin %q")]
    static void CreatePatternWin()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Prefabs/PatternWin.prefab");
        var instance = GameObject.Instantiate(prefab);
        instance.name = "PatternWin";
        if (Selection.activeTransform != null)
        {
            (instance.transform as RectTransform).MatchWhith(Selection.activeTransform as RectTransform);
        }

        Selection.activeObject = instance;
    }

    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
            && Event.current.button == 1 && Event.current.type <= EventType.MouseUp)
        {
            var selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            //这里可以判断selectedGameObject的条件
            if (selectedGameObject)
            {
                var root = selectedGameObject.transform.root;
                if (root.GetComponent<UIRoot>())
                {
                    Vector2 mousePosition = Event.current.mousePosition;
                    EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "GameObject/UICustom", null);
                    Event.current.Use();
                }
            }
        }
    }

}



