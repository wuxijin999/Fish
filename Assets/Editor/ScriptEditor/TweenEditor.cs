using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(Tween), true), CanEditMultipleObjects]
public class TweenEditor : Editor
{
    SerializedProperty m_Type = null;
    SerializedProperty m_IsUI = null;
    SerializedProperty m_IsLocal = null;
    SerializedProperty m_From = null;
    SerializedProperty m_To = null;

    SerializedProperty m_AlphaFrom = null;
    SerializedProperty m_AlphaTo = null;

    SerializedProperty m_Trigger = null;
    SerializedProperty m_WrapMode = null;
    SerializedProperty m_Delay = null;
    SerializedProperty m_Duration = null;

    SerializedProperty m_Ease = null;
    SerializedProperty m_OnComplete;

    private void OnEnable()
    {
        this.m_Type = this.serializedObject.FindProperty("m_Type");
        this.m_IsUI = this.serializedObject.FindProperty("m_IsUI");
        this.m_IsLocal = this.serializedObject.FindProperty("m_IsLocal");
        this.m_From = this.serializedObject.FindProperty("m_From");
        this.m_To = this.serializedObject.FindProperty("m_To");

        this.m_AlphaFrom = this.serializedObject.FindProperty("m_AlphaFrom");
        this.m_AlphaTo = this.serializedObject.FindProperty("m_AlphaTo");

        this.m_Trigger = this.serializedObject.FindProperty("m_Trigger");
        this.m_WrapMode = this.serializedObject.FindProperty("m_WrapMode");
        this.m_Delay = this.serializedObject.FindProperty("m_Delay");
        this.m_Duration = this.serializedObject.FindProperty("m_Duration");

        this.m_Ease = this.serializedObject.FindProperty("m_Ease");
        this.m_OnComplete = this.serializedObject.FindProperty("m_OnComplete");
    }


    public override void OnInspectorGUI()
    {
        var tween = (Tween)this.target;
        EditorGUILayout.Space();
        this.serializedObject.Update();

        EditorGUILayout.PropertyField(this.m_Type, new GUILayoutOption[0]);

        EditorGUI.indentLevel++;
        switch (tween.type)
        {
            case Tween.TweenType.Alpha:
                EditorGUILayout.PropertyField(this.m_AlphaFrom, new GUIContent("From"), new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(this.m_AlphaTo, new GUIContent("To"), new GUILayoutOption[0]);
                break;
            case Tween.TweenType.Position:
            case Tween.TweenType.Rotation:
            case Tween.TweenType.Scale:
                EditorGUILayout.PropertyField(this.m_From, new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(this.m_To, new GUILayoutOption[0]);
                break;
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(this.m_IsUI, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_IsLocal, new GUILayoutOption[0]);

        EditorGUILayout.PropertyField(this.m_Trigger, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_WrapMode, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Delay, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Duration, new GUILayoutOption[0]);

        EditorGUILayout.PropertyField(this.m_Ease, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_OnComplete, new GUILayoutOption[0]);

        this.serializedObject.ApplyModifiedProperties();
    }

}
