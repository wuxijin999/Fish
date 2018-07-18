using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Events;

public class CreateConfigClassFile
{

    const string retract1 = "\t";
    const string retract2 = "\t\t";
    const string retract3 = "\t\t\t";
    const string retract4 = "\t\t\t\t";
    const string retract5 = "\t\t\t\t\t";

    public static event UnityAction FileCreateEvent;

    public static string filedContent = string.Empty;
    public static string readContent = string.Empty;


    [UnityEditor.MenuItem("Assets/生成配置解析类型")]
    public static void GenerateConfigClass()
    {

        if (Selection.objects != null)
        {
            foreach (var o in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(o.GetInstanceID());
                if (path.EndsWith(".txt") || path.EndsWith(".TXT"))
                {
                    CreateConfigClass(new FileInfo(path));
                }
            }

            AssetDatabase.Refresh();
        }
    }

    public static void CreateConfigClass(FileInfo fileInfo)
    {
        var lines = File.ReadAllLines(fileInfo.FullName);
        if (lines.Length > 2)
        {
            var typeLine = lines[0];
            var fieldLine = lines[1];
            var types = typeLine.Split('\t');
            var fields = fieldLine.Split('\t');
            var min = Mathf.Min(types.Length, fields.Length);
            var fieldFulls = new List<string>();
            var readFulls = new List<string>();

            int index = 0;
            for (int j = 0; j < min; j++)
            {
                var type = types[j];
                var field = fields[j];
                var fieldstring = GetField(type, field);
                var readString = GetRead(type, field, index);
                if (!string.IsNullOrEmpty(fieldstring))
                {
                    fieldFulls.Add(fieldstring);
                }

                if (!string.IsNullOrEmpty(readString))
                {
                    index++;
                    readFulls.Add(readString);
                }
            }

            filedContent = string.Join("\r\n\t", fieldFulls.ToArray());
            readContent = string.Join("\r\n\r\n\t\t\t", readFulls.ToArray());
            CreatNewConfigClass(fileInfo.Name.Substring(0, fileInfo.Name.IndexOf('.')));
        }

    }

    public static string GetField(string _type, string _field)
    {
        _field = _field.Replace(" ", "");
        if (_type.Contains("int[]"))
        {
            return StringUtility.Contact("public readonly int[] ", _field.Trim(), ";");
        }
        else if (_type.Contains("float[]"))
        {
            return StringUtility.Contact("public readonly float[] ", _field.Trim(), ";");
        }
        else if (_type.Contains("string[]"))
        {
            return StringUtility.Contact("public readonly string[] ", _field.Trim(), ";");
        }
        else if (_type.Contains("Vector3[]"))
        {
            return StringUtility.Contact("public readonly Vector3[] ", _field.Trim(), ";");
        }
        else if (_type.Contains("int"))
        {
            return StringUtility.Contact("public readonly int ", _field.Trim(), ";");
        }
        else if (_type.Contains("long"))
        {
            return StringUtility.Contact("public readonly long ", _field.Trim(), ";");
        }
        else if (_type.Contains("float"))
        {
            return StringUtility.Contact("public readonly float ", _field.Trim(), ";");
        }
        else if (_type.Contains("string"))
        {
            return StringUtility.Contact("public readonly string ", _field, ";");
        }
        else if (_type.Contains("Vector3"))
        {
            return StringUtility.Contact("public readonly Vector3 ", _field.Trim(), ";");
        }
        else
        {
            return string.Empty;
        }
    }

    public static string GetRead(string _type, string _field, int _index)
    {
        _field = _field.Replace(" ", "");
        if (_type.Contains("int[]"))
        {
            var line1 = StringUtility.Contact("string[] ", _field, "StringArray", " = ", "tables", "[", _index, "]", ".Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtility.Contact(retract3, _field, " = ", "new int", "[", _field, "StringArray.Length]", ";", "\n");
            var line3 = StringUtility.Contact(retract3, "for (int i=0;i<", _field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtility.Contact(retract3, "{\n");
            var line5 = StringUtility.Contact(retract4, " int.TryParse(", _field, "StringArray", "[i]", ",", "out ", _field, "[i]", ")", ";", "\n");
            var line6 = StringUtility.Contact(retract3, "}");

            return StringUtility.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (_type.Contains("float[]"))
        {
            var line1 = StringUtility.Contact("string[] ", _field, "StringArray", " = ", "tables", "[", _index, "]", ".Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtility.Contact(retract3, _field, " = ", "new float", "[", _field, "StringArray.Length", "]", ";", "\n");
            var line3 = StringUtility.Contact(retract3, "for (int i=0;i<", _field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtility.Contact(retract3, "{\n");
            var line5 = StringUtility.Contact(retract4, " float.TryParse(", _field, "StringArray", "[i]", ",", "out ", _field, "[i]", ")", ";", "\n");
            var line6 = StringUtility.Contact(retract3, "}");

            return StringUtility.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (_type.Contains("string[]"))
        {
            var line1 = StringUtility.Contact(_field, " = ", "tables", "[", _index, "]", ".Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);");
            return line1;
        }
        else if (_type.Contains("Vector3[]"))
        {
            var line1 = StringUtility.Contact("string[] ", _field, "StringArray", " = ", "tables", "[", _index, "]", ".Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtility.Contact(retract3, _field, " = ", "new Vector3", "[", _field, "StringArray.Length", "]", ";", "\n");
            var line3 = StringUtility.Contact(retract3, "for (int i=0;i<", _field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtility.Contact(retract3, "{\n");
            var line5 = StringUtility.Contact(retract4, _field, "[i]", "=", _field, "StringArray", "[i]", ".Vector3Parse()", ";", "\n");
            var line6 = StringUtility.Contact(retract3, "}");

            return StringUtility.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (_type.Contains("int"))
        {
            return StringUtility.Contact("int.TryParse(tables", "[", _index, "]", ",", "out ", _field, ")", "; ");
        }
        else if (_type.Contains("float"))
        {
            return StringUtility.Contact("float.TryParse(tables", "[", _index, "]", ",", "out ", _field, ")", "; ");
        }
        else if (_type.Contains("string"))
        {
            return StringUtility.Contact(_field, " = ", "tables", "[", _index, "]", ";");
        }
        else if (_type.Contains("Vector3"))
        {
            return StringUtility.Contact(_field, "=", "tables", "[", _index, "]", ".Vector3Parse()", ";");
        }
        else
        {
            return string.Empty;
        }
    }

    static string configClassPath = "Assets" + "/" + "Scripts/Config";
    static string templatePath = "Assets/Editor/ScriptTemplate/ConfigDataTemplate.txt";

    public static void CreatNewConfigClass(string _name)
    {
        var newConfigPath = configClassPath + string.Format("/{0}Config.cs", _name);
        AssetDatabase.DeleteAsset(newConfigPath);
        UnityEngine.Object o = CreateScriptAssetFromTemplate(newConfigPath, templatePath);
        AddConfigInit(newConfigPath);
        ProjectWindowUtil.ShowCreatedAsset(o);

        if (FileCreateEvent != null)
        {
            FileCreateEvent();
        }
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);

        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#ClassName*#", fileNameWithoutExtension);
        text = Regex.Replace(text, "#DateTime#", System.DateTime.Now.ToLongDateString());
        text = Regex.Replace(text, "#Field#", CreateConfigClassFile.filedContent);
        text = Regex.Replace(text, "#Read#", CreateConfigClassFile.readContent);
        text = Regex.Replace(text, "#FileName#", fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - 6));

        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }

    internal static void AddConfigInit(string _pathName)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_pathName);
        string add = string.Format("{0}.Init();", fileNameWithoutExtension);

        string path = Application.dataPath + "/Scripts/Utility/ConfigInitiator.cs";
        var text = File.ReadAllText(path);

        if (!text.Contains(add))
        {
            text = text.Replace("//初始化结束\r\n", add + "\r\n" + "\t\t//初始化结束\r\n");
        }

        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(path, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(path);

    }
}


