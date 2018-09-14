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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
