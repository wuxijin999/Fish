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

        this.m_Interval = this.serializedObject.FindProperty("m_Interval");
        this.m_PositiveSound = this.serializedObject.FindProperty("m_PositiveSound");
        this.m_NegativeSound = this.serializedObject.FindProperty("m_NegativeSound");
        this.m_TitleMesh = this.serializedObject.FindProperty("m_TitleMesh");
        this.m_NormalColor = this.serializedObject.FindProperty("m_NormalColor");
        this.m_DisableColor = this.serializedObject.FindProperty("m_DisableColor");
        this.m_CoolDownText = this.serializedObject.FindProperty("m_CoolDownText");
        this.m_Image = this.serializedObject.FindProperty("m_Image");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.m_PositiveSound.intValue = EditorGUILayout.IntField("音效", this.m_PositiveSound.intValue);
        this.m_Interval.floatValue = EditorGUILayout.FloatField("点击间隔", this.m_Interval.floatValue);
        if (this.m_Interval.floatValue > 0f)
        {
            EditorGUI.indentLevel++;
            this.m_NegativeSound.intValue = EditorGUILayout.IntField("禁止点击音效", this.m_NegativeSound.intValue);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.m_TitleMesh, new GUIContent("标题"));
        if (this.m_TitleMesh.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            this.m_NormalColor.colorValue = EditorGUILayout.ColorField("普通颜色", this.m_NormalColor.colorValue);
            this.m_DisableColor.colorValue = EditorGUILayout.ColorField("禁用颜色", this.m_DisableColor.colorValue);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.m_CoolDownText, new GUIContent("冷却时间"));
        EditorGUILayout.PropertyField(this.m_Image, new GUIContent("图标"));

        this.serializedObject.ApplyModifiedProperties();

        Repaint();
    }

}
