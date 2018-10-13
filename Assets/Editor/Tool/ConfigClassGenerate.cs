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

    public static string GetField(string type, string field)
    {
        field = field.Replace(" ", "");
        if (type.Contains("int[]"))
        {
            return StringUtil.Contact("public readonly int[] ", field.Trim(), ";");
        }
        else if (type.Contains("Int2[]"))
        {
            return StringUtil.Contact("public readonly Int2[] ", field.Trim(), ";");
        }
        else if (type.Contains("Int3[]"))
        {
            return StringUtil.Contact("public readonly Int3[] ", field.Trim(), ";");
        }
        else if (type.Contains("float[]"))
        {
            return StringUtil.Contact("public readonly float[] ", field.Trim(), ";");
        }
        else if (type.Contains("string[]"))
        {
            return StringUtil.Contact("public readonly string[] ", field.Trim(), ";");
        }
        else if (type.Contains("Vector3[]"))
        {
            return StringUtil.Contact("public readonly Vector3[] ", field.Trim(), ";");
        }
        else if (type.Contains("int"))
        {
            return StringUtil.Contact("public readonly int ", field.Trim(), ";");
        }
        else if (type.Contains("long"))
        {
            return StringUtil.Contact("public readonly long ", field.Trim(), ";");
        }
        else if (type.Contains("float"))
        {
            return StringUtil.Contact("public readonly float ", field.Trim(), ";");
        }
        else if (type.Contains("string"))
        {
            return StringUtil.Contact("public readonly string ", field, ";");
        }
        else if (type.Contains("Vector3"))
        {
            return StringUtil.Contact("public readonly Vector3 ", field.Trim(), ";");
        }
        else if (type.Contains("bool"))
        {
            return StringUtil.Contact("public readonly bool ", field.Trim(), ";");
        }
        else if (type.Contains("Int2"))
        {
            return StringUtil.Contact("public readonly Int2 ", field.Trim(), ";");
        }
        else if (type.Contains("Int3"))
        {
            return StringUtil.Contact("public readonly Int3 ", field.Trim(), ";");
        }
        else
        {
            return string.Empty;
        }
    }

    public static string GetRead(string type, string field, int index)
    {
        field = field.Replace(" ", "");
        if (type.Contains("int[]"))
        {
            var line1 = StringUtil.Contact("string[] ", field, "StringArray", " = ", "tables", "[", index, "]", ".Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtil.Contact(retract3, field, " = ", "new int", "[", field, "StringArray.Length]", ";", "\n");
            var line3 = StringUtil.Contact(retract3, "for (int i=0;i<", field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtil.Contact(retract3, "{\n");
            var line5 = StringUtil.Contact(retract4, " int.TryParse(", field, "StringArray", "[i]", ",", "out ", field, "[i]", ")", ";", "\n");
            var line6 = StringUtil.Contact(retract3, "}");

            return StringUtil.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (type.Contains("Int2[]"))
        {
            var line1 = StringUtil.Contact("string[] ", field, "StringArray", " = ", "tables", "[", index, "]", ".Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtil.Contact(retract3, field, " = ", "new Int2", "[", field, "StringArray.Length]", ";", "\n");
            var line3 = StringUtil.Contact(retract3, "for (int i=0;i<", field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtil.Contact(retract3, "{\n");
            var line5 = StringUtil.Contact(retract4, " Int2.TryParse(", field, "StringArray", "[i]", ",", "out ", field, "[i]", ")", ";", "\n");
            var line6 = StringUtil.Contact(retract3, "}");

            return StringUtil.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (type.Contains("Int3[]"))
        {
            var line1 = StringUtil.Contact("string[] ", field, "StringArray", " = ", "tables", "[", index, "]", ".Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtil.Contact(retract3, field, " = ", "new Int3", "[", field, "StringArray.Length]", ";", "\n");
            var line3 = StringUtil.Contact(retract3, "for (int i=0;i<", field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtil.Contact(retract3, "{\n");
            var line5 = StringUtil.Contact(retract4, " Int3.TryParse(", field, "StringArray", "[i]", ",", "out ", field, "[i]", ")", ";", "\n");
            var line6 = StringUtil.Contact(retract3, "}");

            return StringUtil.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (type.Contains("float[]"))
        {
            var line1 = StringUtil.Contact("string[] ", field, "StringArray", " = ", "tables", "[", index, "]", ".Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtil.Contact(retract3, field, " = ", "new float", "[", field, "StringArray.Length", "]", ";", "\n");
            var line3 = StringUtil.Contact(retract3, "for (int i=0;i<", field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtil.Contact(retract3, "{\n");
            var line5 = StringUtil.Contact(retract4, " float.TryParse(", field, "StringArray", "[i]", ",", "out ", field, "[i]", ")", ";", "\n");
            var line6 = StringUtil.Contact(retract3, "}");

            return StringUtil.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (type.Contains("string[]"))
        {
            var line1 = StringUtil.Contact(field, " = ", "tables", "[", index, "]", ".Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);");
            return line1;
        }
        else if (type.Contains("Vector3[]"))
        {
            var line1 = StringUtil.Contact("string[] ", field, "StringArray", " = ", "tables", "[", index, "]", ".Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);", "\n");
            var line2 = StringUtil.Contact(retract3, field, " = ", "new Vector3", "[", field, "StringArray.Length", "]", ";", "\n");
            var line3 = StringUtil.Contact(retract3, "for (int i=0;i<", field, "StringArray", ".Length", ";", "i++", ")", "\n");
            var line4 = StringUtil.Contact(retract3, "{\n");
            var line5 = StringUtil.Contact(retract4, field, "[i]", "=", field, "StringArray", "[i]", ".Vector3Parse()", ";", "\n");
            var line6 = StringUtil.Contact(retract3, "}");

            return StringUtil.Contact(line1, line2, line3, line4, line5, line6);
        }
        else if (type.Contains("int"))
        {
            return StringUtil.Contact("int.TryParse(tables", "[", index, "]", ",", "out ", field, ")", "; ");
        }
        else if (type.Contains("float"))
        {
            return StringUtil.Contact("float.TryParse(tables", "[", index, "]", ",", "out ", field, ")", "; ");
        }
        else if (type.Contains("string"))
        {
            return StringUtil.Contact(field, " = ", "tables", "[", index, "]", ";");
        }
        else if (type.Contains("Vector3"))
        {
            return StringUtil.Contact(field, "=", "tables", "[", index, "]", ".Vector3Parse()", ";");
        }
        else if (type.Contains("bool"))
        {
            var line1 = StringUtil.Contact("var ", field, "Temp", " = 0", ";", "\n");
            var line2 = StringUtil.Contact(retract3, "int.TryParse(tables", "[", index, "]", ",", "out ", field, "Temp", ")", "; ", "\n");
            var line3 = StringUtil.Contact(retract3, field, "=", field, "Temp", "!=0", ";");
            return StringUtil.Contact(line1, line2, line3);
        }
        else if (type.Contains("Int2"))
        {
            return StringUtil.Contact("Int2.TryParse(tables", "[", index, "]", ",", "out ", field, ")", "; ");
        }
        else if (type.Contains("Int3"))
        {
            return StringUtil.Contact("Int3.TryParse(tables", "[", index, "]", ",", "out ", field, ")", "; ");
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

    internal static void AddConfigInit(string pathName)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
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


