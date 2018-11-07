using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;

[CustomEditor(typeof(TextEx)), CanEditMultipleObjects]
public class TextExEditor : TMP_UiEditorPanel
{

    SerializedProperty m_LanguageKey;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.m_LanguageKey = this.m_LanguageKey ?? (this.m_LanguageKey = this.serializedObject.FindProperty("m_LanguageKey"));
        this.m_LanguageKey.intValue = EditorGUILayout.IntField("语言key", this.m_LanguageKey.intValue);

        this.serializedObject.ApplyModifiedProperties();
        Repaint();
    }
}
