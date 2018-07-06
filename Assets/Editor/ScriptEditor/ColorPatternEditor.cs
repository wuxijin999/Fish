using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ColorPattern))]
public class ColorPatternEditor : Editor
{
    SerializedProperty m_White;
    SerializedProperty m_Green;
    SerializedProperty m_Blue;
    SerializedProperty m_Purple;
    SerializedProperty m_Orange;
    SerializedProperty m_Red;
    SerializedProperty m_Pink;
    SerializedProperty m_Gray;

    private void OnEnable()
    {
        m_White = serializedObject.FindProperty("m_White");
        m_Green = serializedObject.FindProperty("m_Green");
        m_Blue = serializedObject.FindProperty("m_Blue");
        m_Purple = serializedObject.FindProperty("m_Purple");
        m_Orange = serializedObject.FindProperty("m_Orange");
        m_Red = serializedObject.FindProperty("m_Red");
        m_Pink = serializedObject.FindProperty("m_Pink");
        m_Gray = serializedObject.FindProperty("m_Gray");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        base.serializedObject.Update();

        EditorGUILayout.PropertyField(m_White, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Green, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Blue, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Purple, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Orange, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Red, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Pink, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Gray, new GUILayoutOption[0]);

        base.serializedObject.ApplyModifiedProperties();
    }


}
