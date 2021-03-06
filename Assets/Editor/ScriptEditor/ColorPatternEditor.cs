﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorPattern))]
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
        this.m_White = this.serializedObject.FindProperty("m_White");
        this.m_Green = this.serializedObject.FindProperty("m_Green");
        this.m_Blue = this.serializedObject.FindProperty("m_Blue");
        this.m_Purple = this.serializedObject.FindProperty("m_Purple");
        this.m_Orange = this.serializedObject.FindProperty("m_Orange");
        this.m_Red = this.serializedObject.FindProperty("m_Red");
        this.m_Pink = this.serializedObject.FindProperty("m_Pink");
        this.m_Gray = this.serializedObject.FindProperty("m_Gray");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        base.serializedObject.Update();

        EditorGUILayout.PropertyField(this.m_White, new GUIContent("标准白"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Green, new GUIContent("标准绿"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Blue, new GUIContent("标准蓝"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Purple, new GUIContent("标准紫"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Orange, new GUIContent("标准橙"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Red, new GUIContent("标准红"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Pink, new GUIContent("标准粉"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Gray, new GUIContent("标准灰"), new GUILayoutOption[0]);

        base.serializedObject.ApplyModifiedProperties();
    }


}
