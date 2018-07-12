using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIPoolDebugWindow : EditorWindow
{
    public Dictionary<int, UIGameObjectPool> pools = null;

    Vector2 scrollPosition;

    private void OnGUI()
    {
        if (pools == null)
        {
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (var pool in pools.Values)
        {
            EditorGUILayout.Space();
            DrawPool(pool);
        }
        foreach (var pool in pools.Values)
        {
            GUILayout.Space(20);
            DrawPool(pool);
        }
        foreach (var pool in pools.Values)
        {
            GUILayout.Space(20);
            DrawPool(pool);
        }
        foreach (var pool in pools.Values)
        {
            GUILayout.Space(20);
            DrawPool(pool);
        }

        EditorGUILayout.EndScrollView();
    }

    Dictionary<int, ViewSwitch> switches = new Dictionary<int, ViewSwitch>();

    private void DrawPool(UIGameObjectPool _pool)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(StringUtility.Contact("ID:", _pool.instanceId), GUILayout.MaxWidth(100));
        EditorGUILayout.ObjectField("Root", _pool.root, typeof(GameObject), true, GUILayout.MaxWidth(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        ViewSwitch viewSwitch = null;
        if (!switches.ContainsKey(_pool.instanceId))
        {
            viewSwitch = switches[_pool.instanceId] = new ViewSwitch(_pool.instanceId);
        }
        else
        {
            viewSwitch = switches[_pool.instanceId];
        }

        viewSwitch.active = EditorGUILayout.Toggle("使用的", viewSwitch.active);
        EditorGUILayout.EndHorizontal();

        if (viewSwitch.active)
        {
            var freeList = _pool.GetActiveList();
            EditorGUI.indentLevel++;
            for (int i = 0; i < freeList.Count; i++)
            {
                var element = freeList[i];
                EditorGUILayout.ObjectField(StringUtility.Contact("Element", i + 1), element, typeof(GameObject), true);
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.BeginHorizontal();
        viewSwitch.free = EditorGUILayout.Toggle("备用的", viewSwitch.free);
        EditorGUILayout.EndHorizontal();

        if (viewSwitch.free)
        {
            var poolList = _pool.GetFreeList();
            EditorGUI.indentLevel++;
            for (int i = 0; i < poolList.Count; i++)
            {
                var element = poolList[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(StringUtility.Contact("Element", i + 1), element, typeof(GameObject), true);
                EditorGUILayout.LabelField(element == null || element.transform.parent != _pool.root.transform ? "Error" : "");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

    }

    class ViewSwitch
    {
        public readonly int poolInstance;
        public bool active = false;
        public bool free = false;

        public ViewSwitch(int _instance)
        {
            poolInstance = _instance;
        }

    }


#if UNITY_EDITOR
    [MenuItem("Tools/UI 对象池")]
    public static void CreatePoolDebugWindow()
    {
        var window = GetWindow(typeof(UIPoolDebugWindow), false, "对象池Debug") as UIPoolDebugWindow;
        window.Show();
        window.pools = UIGameObjectPoolUtility.pools;
        window.autoRepaintOnSceneChange = true;
    }
#endif
}
