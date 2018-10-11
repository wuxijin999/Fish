using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtensionalTools
{

    public static string shaderEditorPath {
        get { return LocalSave.GetString("ExtensionalTools_ShaderEditor"); }
        set { LocalSave.SetString("ExtensionalTools_ShaderEditor", value); }
    }

    public static string txtEditorPath {
        get { return LocalSave.GetString("ExtensionalTools_TxtEditor"); }
        set { LocalSave.SetString("ExtensionalTools_TxtEditor", value); }
    }

    public static string excelRootPath {
        get { return LocalSave.GetString("ExtensionalTools_ExcelRootPath"); }
        set { LocalSave.SetString("ExtensionalTools_ExcelRootPath", value); }
    }

}

public class ExtensionalToolsWindow : EditorWindow
{

    static ExtensionalToolsWindow window;
    [MenuItem("Tools/外部工具")]
    public static void Open()
    {
        window = GetWindow(typeof(ExtensionalToolsWindow), false, "外部工具") as ExtensionalToolsWindow;
        window.Show();
        window.autoRepaintOnSceneChange = true;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        ExtensionalTools.shaderEditorPath = EditorGUILayout.TextField("Shader编辑器", ExtensionalTools.shaderEditorPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        ExtensionalTools.txtEditorPath = EditorGUILayout.TextField("文本文件编辑器", ExtensionalTools.txtEditorPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        ExtensionalTools.excelRootPath = EditorGUILayout.TextField("Excel表根目录", ExtensionalTools.excelRootPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
    }

}