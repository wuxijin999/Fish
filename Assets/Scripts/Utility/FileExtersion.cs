using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileExtension
{
    public readonly static string[] lineSplit = new string[] { "\r\n" }; //行间隔体

    public static List<FileInfo> GetFileInfos(string path, string[] searchPatterns)
    {
        if (searchPatterns == null)
        {
            return null;
        }

        var fileInfoes = new List<FileInfo>();
        var directoryInfo = new DirectoryInfo(path);

        for (int i = 0; i < searchPatterns.Length; i++)
        {
            fileInfoes.AddRange(directoryInfo.GetFiles(searchPatterns[i], SearchOption.AllDirectories));
        }

        return fileInfoes;
    }

    public static void GetAllDirectoryFileInfos(string path, ref List<FileInfo> fileInfos)
    {
        var directoryInfo = new DirectoryInfo(path);
        var allFileSystemInfos = directoryInfo.GetFileSystemInfos();

        for (int i = 0; i < allFileSystemInfos.Length; i++)
        {
            var fileSystemInfo = allFileSystemInfos[i];
            if (fileSystemInfo is DirectoryInfo)
            {
                GetAllDirectoryFileInfos(fileSystemInfo.FullName, ref fileInfos);
            }

            if (fileSystemInfo is FileInfo)
            {
                fileInfos.Add(fileSystemInfo as FileInfo);
            }
        }
    }

    public static string GetFileRelativePath(string root, string fileFullName)
    {
        root = root.Replace("\\", "/");
        fileFullName = fileFullName.Replace("\\", "/");
        var relativePath = fileFullName.Replace(root, "");
        if (relativePath.StartsWith("/"))
        {
            relativePath = relativePath.Substring(1, relativePath.Length - 1);
        }

        return relativePath;
    }

    public static void DirectoryCopy(string from, string to)
    {
        if (!Directory.Exists(from))
        {
            return;
        }

        if (!Directory.Exists(to))
        {
            Directory.CreateDirectory(to);
        }

        var files = Directory.GetFiles(from);
        foreach (var formFileName in files)
        {
            string fileName = Path.GetFileName(formFileName);
            string toFileName = Path.Combine(to, fileName);
            File.Copy(formFileName, toFileName, true);
        }

        var fromDirs = Directory.GetDirectories(from);
        foreach (var fromDirName in fromDirs)
        {
            var dirName = Path.GetFileName(fromDirName);
            var toDirName = Path.Combine(to, dirName);
            DirectoryCopy(fromDirName, toDirName);
        }
    }

    public static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            var file = new FileStream(fileName, System.IO.FileMode.Open);
            var md5 = new MD5CryptoServiceProvider();
            var retVal = md5.ComputeHash(file);
            file.Close();
            var sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    public static void CreateDirectory(string path)
    {
        var directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

}
