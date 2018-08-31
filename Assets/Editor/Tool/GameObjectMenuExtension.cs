using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameObjectMenuExtension
{
    [MenuItem("GameObject/UICustom/Button")]
    static void Test()
    {
        Debug.Log("创建一个button");
    }

    [MenuItem("GameObject/UICustom/Image")]
    static void Test1()
    {
        Debug.Log("创建一个Image");
    }

    [MenuItem("GameObject/UICustom/Text")]
    static void Test2()
    {
        Debug.Log("创建一个Text");
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
                var root = selectedGameObject.transform.GetRoot();
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



