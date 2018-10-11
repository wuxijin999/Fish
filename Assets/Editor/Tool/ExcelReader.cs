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

    [MenuItem("Assets/Í¬²½ExcelÄ¸±í")]
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
                var excelPath = StringUtil.Contact(Path.GetDirectoryName(path), "/", Path.GetFileNameWithoutExtension(path), ".xlsx");
                var lines = ExcelRead2(excelPath);
                File.WriteAllLines(path, lines.ToArray());
            }
        }

    }

    static List<string> ExcelRead2(string excelPath)
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
