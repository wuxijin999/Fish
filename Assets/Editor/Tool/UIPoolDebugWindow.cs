using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIPoolDebugWindow : EditorWindow
{
    public Dictionary<int, GameObjectPool> pools = null;

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

        EditorGUILayout.EndScrollView();
    }

    Dictionary<int, ViewSwitch> switches = new Dictionary<int, ViewSwitch>();

    private void DrawPool(GameObjectPool pool)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(StringUtil.Contact("ID:", pool.instanceId), GUILayout.MaxWidth(100));
        EditorGUILayout.ObjectField("Root", pool.root, typeof(GameObject), true, GUILayout.MaxWidth(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        ViewSwitch viewSwitch = null;
        if (!switches.ContainsKey(pool.instanceId))
        {
            viewSwitch = switches[pool.instanceId] = new ViewSwitch(pool.instanceId);
        }
        else
        {
            viewSwitch = switches[pool.instanceId];
        }

        viewSwitch.active = EditorGUILayout.Toggle("使用的", viewSwitch.active);
        EditorGUILayout.EndHorizontal();

        if (viewSwitch.active)
        {
            var freeList = pool.GetActiveList();
            EditorGUI.indentLevel++;
            for (int i = 0; i < freeList.Count; i++)
            {
                var element = freeList[i];
                EditorGUILayout.ObjectField(StringUtil.Contact("Element", i + 1), element, typeof(GameObject), true);
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.BeginHorizontal();
        viewSwitch.free = EditorGUILayout.Toggle("备用的", viewSwitch.free);
        EditorGUILayout.EndHorizontal();

        if (viewSwitch.free)
        {
            var poolList = pool.GetFreeList();
            EditorGUI.indentLevel++;
            for (int i = 0; i < poolList.Count; i++)
            {
                var element = poolList[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(StringUtil.Contact("Element", i + 1), element, typeof(GameObject), true);
                EditorGUILayout.LabelField(element == null || element.transform.parent != pool.root.transform ? "Error" : "");
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
        GameObjectPoolUtil.CreatePoolDebugWindow();
        var window = GetWindow(typeof(UIPoolDebugWindow), false, "对象池Debug") as UIPoolDebugWindow;
        window.Show();
        window.pools = EditorHelper.gameObjectPools;
        window.autoRepaintOnSceneChange = true;
    }
#endif

}
