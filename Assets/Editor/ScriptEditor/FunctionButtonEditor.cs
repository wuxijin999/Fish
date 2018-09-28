using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

[CustomEditor(typeof(FunctionButton), true), CanEditMultipleObjects]
public class FunctionButtonEditor : UnityEditor.UI.ButtonEditor
{

    //        int  m_Order = 0;
    //          int m_FunctionId = -1;
    //          State m_State = State.Normal;
    //          Button m_Button;
    //          ImageEx m_Icon;
    //          TextEx m_Title;
    //          RectTransform m_Locked;
    //          int m_Audio = 1;
    //          FunctionButtonGroup m_Group;


    SerializedProperty order;
    SerializedProperty functionId;
    SerializedProperty button;
    SerializedProperty icon;
    SerializedProperty title;
    SerializedProperty locked;
    SerializedProperty audio;
    SerializedProperty group;

    protected override void OnEnable()
    {
        base.OnEnable();

        order = this.serializedObject.FindProperty("m_Order");
        functionId = this.serializedObject.FindProperty("m_FunctionId");
        button = this.serializedObject.FindProperty("m_Button");
        icon = this.serializedObject.FindProperty("m_Icon");
        title = this.serializedObject.FindProperty("m_Title");
        locked = this.serializedObject.FindProperty("m_Locked");
        audio = this.serializedObject.FindProperty("m_Audio");
        group = this.serializedObject.FindProperty("m_Group");

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        this.order.intValue = EditorGUILayout.IntField("Order", this.order.intValue);
        this.functionId.intValue = EditorGUILayout.IntField("FunctionId", this.functionId.intValue);
        this.audio.intValue = EditorGUILayout.IntField("Audio", this.order.intValue);

        EditorGUILayout.PropertyField(this.button, new GUIContent("Button"));
        EditorGUILayout.PropertyField(this.icon, new GUIContent("Icon"));
        EditorGUILayout.PropertyField(this.title, new GUIContent("Title"));
        EditorGUILayout.PropertyField(this.locked, new GUIContent("Lock"));
        EditorGUILayout.PropertyField(this.group, new GUIContent("Group"));

        base.serializedObject.ApplyModifiedProperties();
    }

}


