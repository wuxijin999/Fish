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
        m_Type = serializedObject.FindProperty("m_Type");
        m_IsUI = serializedObject.FindProperty("m_IsUI");
        m_IsLocal = serializedObject.FindProperty("m_IsLocal");
        m_From = serializedObject.FindProperty("m_From");
        m_To = serializedObject.FindProperty("m_To");

        m_AlphaFrom = serializedObject.FindProperty("m_AlphaFrom");
        m_AlphaTo = serializedObject.FindProperty("m_AlphaTo");

        m_Trigger = serializedObject.FindProperty("m_Trigger");
        m_WrapMode = serializedObject.FindProperty("m_WrapMode");
        m_Delay = serializedObject.FindProperty("m_Delay");
        m_Duration = serializedObject.FindProperty("m_Duration");

        m_Ease = serializedObject.FindProperty("m_Ease");
        m_OnComplete = serializedObject.FindProperty("m_OnComplete");
    }


    public override void OnInspectorGUI()
    {
        var tween = (Tween)target;
        EditorGUILayout.Space();
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_Type, new GUILayoutOption[0]);

        EditorGUI.indentLevel++;
        switch (tween.type)
        {
            case Tween.TweenType.Alpha:
                EditorGUILayout.PropertyField(m_AlphaFrom, new GUIContent("From"), new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(m_AlphaTo, new GUIContent("To"), new GUILayoutOption[0]);
                break;
            case Tween.TweenType.Position:
            case Tween.TweenType.Rotation:
            case Tween.TweenType.Scale:
                EditorGUILayout.PropertyField(m_From, new GUILayoutOption[0]);
                EditorGUILayout.PropertyField(m_To, new GUILayoutOption[0]);
                break;
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(m_IsUI, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_IsLocal, new GUILayoutOption[0]);

        EditorGUILayout.PropertyField(m_Trigger, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_WrapMode, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Delay, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_Duration, new GUILayoutOption[0]);

        EditorGUILayout.PropertyField(m_Ease, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_OnComplete, new GUILayoutOption[0]);

        serializedObject.ApplyModifiedProperties();
    }

}
