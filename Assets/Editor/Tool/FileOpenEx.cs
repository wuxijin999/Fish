﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class FileOpenEx
{

    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line)
    {
        return false;
    }

    // step2 has an attribute with index 2, so will be called after step1
    [OnOpenAssetAttribute(2)]
    public static bool step2(int instanceID, int line)
    {
        string path = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));
        string name = Application.dataPath + "/" + path.Replace("Assets/", "");

        if (name.EndsWith(".Shader") || name.EndsWith(".cginc") || name.EndsWith(".shader"))
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = ExtensionalTools.shaderEditorPath;
            startInfo.Arguments = name;
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }
        else if (name.EndsWith(".txt") && name.Contains("5_Config"))
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = ExtensionalTools.txtEditorPath;

            name = ExcelReader.GetExcelPath(Path.GetFileNameWithoutExtension(path));
            startInfo.Arguments = name;
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }

        return false;
    }
}
