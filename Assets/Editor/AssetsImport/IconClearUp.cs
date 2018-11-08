using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class IconClearUp
{

    [MenuItem("Tools/Asset Import/UI/初始化Icon表")]
    public static void FillIconTable()
    {
        var path = AssetPath.UI_SPRITE_ROOT_PATH;
        var files = new List<FileInfo>();
        FileExtension.GetAllDirectoryFileInfos(path, ref files);

        var lines = new List<string>();
        lines.Add(StringUtil.Contact("int", "\t", "string", "\t", "string"));
        lines.Add(StringUtil.Contact("id", "\t", "folder", "\t", "assetName"));
        lines.Add(StringUtil.Contact("icon id", "\t", "图片文件夹", "\t", "资源名称"));

        var id = 0;
        foreach (var item in files)
        {
            var extension = Path.GetExtension(item.FullName);
            if (extension == ".png")
            {
                var fileName = Path.GetFileNameWithoutExtension(item.FullName);
                var temp = item.FullName.Split(Path.DirectorySeparatorChar);
                var directoryName = temp[temp.Length - 2];

                lines.Add(StringUtil.Contact(++id, "\t", directoryName, "\t", fileName));
            }
        }

        File.WriteAllLines(AssetPath.CONFIG_ROOT_PATH + "Icon.txt", lines.ToArray());
    }


}
