using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using UnityEditor;

public class ExcelReader
{

    static Dictionary<string, string> txtExcelTables = new Dictionary<string, string>();

    public static string GetExcelPath(string txtName)
    {
        if (!txtExcelTables.ContainsKey(txtName))
        {
            var lines = File.ReadAllLines(Application.dataPath + "/Editor/Config/ExcelToTxt.txt");
            for (var i = 1; i < lines.Length; i++)
            {
                var contents = lines[i].Split('\t');
                txtExcelTables[contents[1]] = contents[0];
            }
        }

        if (!txtExcelTables.ContainsKey(txtName))
        {
            return string.Empty;
        }

        return StringUtil.Contact(ExtensionalTools.excelRootPath, "/", txtExcelTables[txtName], ".xlsx");
    }

    [MenuItem("Assets/同步Excel母表")]
    static void XLSX()
    {
        if (Selection.objects == null)
        {
            return;
        }

        foreach (var item in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(item);
            path = Application.dataPath + path.Substring(6, path.Length - 6);
            var extension = Path.GetExtension(path);
            if (extension.ToLower() == ".txt")
            {
                var excelPath = GetExcelPath(Path.GetFileNameWithoutExtension(path));
                var lines = ExcelRead(excelPath);
                File.WriteAllLines(path, lines.ToArray(), Encoding.UTF8);
            }
        }
    }

    static List<string> ExcelRead(string excelPath)
    {
        var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
        var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        var result = excelReader.AsDataSet();

        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;

        var contents = new Dictionary<int, Dictionary<int, string>>();
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var isClient = result.Tables[0].Rows[0][j].ToString().ToLower().Contains("c");
                var nvalue = result.Tables[0].Rows[i][j].ToString();
                if (isClient)
                {
                    var lineContents = contents.ContainsKey(i) ? contents[i] : contents[i] = new Dictionary<int, string>();
                    lineContents[j] = nvalue;
                }
            }
        }

        var lines = new List<string>();
        foreach (var item in contents.Values)
        {
            lines.Add(string.Join("\t", item.Values.ToArray()));
        }

        lines.RemoveAt(0);
        return lines;
    }

}
