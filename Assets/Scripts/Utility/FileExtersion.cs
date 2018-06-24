using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileExtersion
{
    public static string[] lineSplit = new string[] { "\r\n" }; //行间隔体

    public static List<FileInfo> GetFileInfos(string _path, string[] _searchPatterns)
    {
        if (_searchPatterns == null)
        {
            return null;
        }

        var fileInfoes = new List<FileInfo>();
        var directoryInfo = new DirectoryInfo(_path);

        for (int i = 0; i < _searchPatterns.Length; i++)
        {
            fileInfoes.AddRange(directoryInfo.GetFiles(_searchPatterns[i], SearchOption.AllDirectories));
        }

        return fileInfoes;
    }

    public static void GetAllDirectoryFileInfos(string _path, List<FileInfo> _fileInfos)
    {
        var directoryInfo = new DirectoryInfo(_path);
        var allFileSystemInfos = directoryInfo.GetFileSystemInfos();

        for (int i = 0; i < allFileSystemInfos.Length; i++)
        {
            var fileSystemInfo = allFileSystemInfos[i];
            if (fileSystemInfo is DirectoryInfo)
            {
                GetAllDirectoryFileInfos(fileSystemInfo.FullName, _fileInfos);
            }

            if (fileSystemInfo is FileInfo)
            {
                _fileInfos.Add(fileSystemInfo as FileInfo);
            }
        }
    }

    public static string GetFileRelativePath(string _root, string _fileFullName)
    {
        _root = _root.Replace("\\", "/");
        _fileFullName = _fileFullName.Replace("\\", "/");
        var relativePath = _fileFullName.Replace(_root, "");
        if (relativePath.StartsWith("/"))
        {
            relativePath = relativePath.Substring(1, relativePath.Length - 1);
        }

        return relativePath;
    }

    public static int GetVersionFromFileName(string _fileName)
    {
        var index = _fileName.IndexOf("_version_");

        if (index == -1)
        {
            return 0;
        }
        else
        {
            var version = 0;
            int.TryParse(_fileName.Substring(0, index), out version);
            return version;
        }
    }

    public static string RemoveVersionFromFileFullName(string _fullName)
    {
        var fileName = Path.GetFileName(_fullName);
        var index = fileName.IndexOf("_version_");

        if (index != -1)
        {
            var startIndex = index + 9;
            fileName = fileName.Substring(startIndex, fileName.Length - startIndex);
            return StringUtility.Contact(Path.GetDirectoryName(_fullName), Path.DirectorySeparatorChar, fileName);
        }
        else
        {
            return _fullName;
        }
    }

    public static string AddVersionToFileFullName(string _fullName, int _version)
    {
        var fileName = Path.GetFileName(_fullName);
        var index = fileName.IndexOf("_version_");

        if (index != -1)
        {
            fileName = fileName.Substring(0, index);
        }

        return StringUtility.Contact(Path.GetDirectoryName(_fullName), Path.DirectorySeparatorChar, _version, "_version_", fileName);
    }

    public static void DirectoryCopy(string _from, string _to)
    {
        if (!Directory.Exists(_from))
        {
            return;
        }

        if (!Directory.Exists(_to))
        {
            Directory.CreateDirectory(_to);
        }

        var files = Directory.GetFiles(_from);
        foreach (var formFileName in files)
        {
            string fileName = Path.GetFileName(formFileName);
            string toFileName = Path.Combine(_to, fileName);
            File.Copy(formFileName, toFileName,true);
        }

        var fromDirs = Directory.GetDirectories(_from);
        foreach (var fromDirName in fromDirs)
        {
            var dirName = Path.GetFileName(fromDirName);
            var toDirName = Path.Combine(_to, dirName);
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
}
