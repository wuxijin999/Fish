using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(ButtonEx), false)]
public class ButtonExEditor : UnityEditor.UI.ButtonEditor
{
    SerializedProperty m_Interval;
    SerializedProperty m_PositiveSound;
    SerializedProperty m_NegativeSound;
    SerializedProperty m_TitleMesh;
    SerializedProperty m_NormalColor;
    SerializedProperty m_DisableColor;
    SerializedProperty m_CoolDownText;
    SerializedProperty m_Image;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_Interval = serializedObject.FindProperty("m_Interval");
        m_PositiveSound = serializedObject.FindProperty("m_PositiveSound");
        m_NegativeSound = serializedObject.FindProperty("m_NegativeSound");
        m_TitleMesh = serializedObject.FindProperty("m_TitleMesh");
        m_NormalColor = serializedObject.FindProperty("m_NormalColor");
        m_DisableColor = serializedObject.FindProperty("m_DisableColor");
        m_CoolDownText = serializedObject.FindProperty("m_CoolDownText");
        m_Image = serializedObject.FindProperty("m_Image");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        m_PositiveSound.intValue = EditorGUILayout.IntField("音效", m_PositiveSound.intValue);
        m_Interval.floatValue = EditorGUILayout.FloatField("点击间隔", m_Interval.floatValue);
        if (m_Interval.floatValue > 0f)
        {
            EditorGUI.indentLevel++;
            m_NegativeSound.intValue = EditorGUILayout.IntField("禁止点击音效", m_NegativeSound.intValue);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_TitleMesh, new GUIContent("标题"));
        if (m_TitleMesh.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            m_NormalColor.colorValue = EditorGUILayout.ColorField("普通颜色", m_NormalColor.colorValue);
            m_DisableColor.colorValue = EditorGUILayout.ColorField("禁用颜色", m_DisableColor.colorValue);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_CoolDownText, new GUIContent("冷却时间"));
        EditorGUILayout.PropertyField(m_Image, new GUIContent("图标"));

        serializedObject.ApplyModifiedProperties();

        Repaint();
    }

}
