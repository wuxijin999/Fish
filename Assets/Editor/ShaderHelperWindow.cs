using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderHelperWindow : ScriptableWizard
{

    static ShaderHelperWindow window;
    [MenuItem("Shader/调色板")]
    public static void Open()
    {
        window = GetWindowWithRect<ShaderHelperWindow>(new Rect(400, 100, 400, 200), true);
        window.Show();
        window.autoRepaintOnSceneChange = true;
    }

    private void OnGUI()
    {
        Shader.SetGlobalColor("_Global_LightColor", EditorGUILayout.ColorField("全局光照颜色", Shader.GetGlobalColor("_Global_LightColor")));
        Shader.SetGlobalColor("_Gbl_Pnt", EditorGUILayout.ColorField("全局点光源颜色", Shader.GetGlobalColor("_Gbl_Pnt")));
        Shader.SetGlobalColor("_Gbl_Amb", EditorGUILayout.ColorField("全局漫反射颜色", Shader.GetGlobalColor("_Gbl_Amb")));
        Shader.SetGlobalColor("_Gbl_Spc", EditorGUILayout.ColorField("全局高光颜色", Shader.GetGlobalColor("_Gbl_Spc")));
        Shader.SetGlobalColor("_Gbl_Rim", EditorGUILayout.ColorField("全局边缘光颜色", Shader.GetGlobalColor("_Gbl_Rim")));
        Shader.SetGlobalColor("_Gbl_Wat", EditorGUILayout.ColorField("全局水颜色", Shader.GetGlobalColor("_Gbl_Wat")));

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("应用"))
        {
        }

        EditorGUILayout.EndHorizontal();
    }





}
