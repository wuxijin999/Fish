using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetVersion
{
    string m_RelativePath = string.Empty;
    public string relativePath { get { return this.m_RelativePath; } }

    string m_FileName;
    public string fileName { get { return this.m_FileName; } }

    string m_Extension = string.Empty;
    public string extension { get { return this.m_Extension; } }

    string m_Md5 = string.Empty;
    public string md5 { get { return this.m_Md5; } }

    int m_Size = 0;
    public int size { get { return this.m_Size; } }       //字节     

    StorageLocation m_FileLocation = StorageLocation.ExternalStore;
    public StorageLocation fileLocation { get { return this.m_FileLocation; } }

    bool m_LocalValid = false;
    public bool localValid {
        get { return this.m_LocalValid; }
        set { this.m_LocalValid = value; }
    }

    public AssetVersion(string versionString)
    {
        var strings = versionString.Split('\t');

        this.m_RelativePath = strings[0];
        this.m_Extension = strings[1];
        int.TryParse(strings[2], out this.m_Size);
        this.m_Md5 = strings[3];

        var paths = this.m_RelativePath.Split('/');

        var lastPath = paths[paths.Length - 1];
        var index = lastPath.IndexOf('.');
        this.m_FileName = index != -1 ? lastPath.Substring(0, index) : lastPath;
    }

    public AssetCategory GetAssetCategory()
    {
        if (this.extension == ".dll")
        {
            return AssetCategory.Dll;
        }
        else if (this.m_RelativePath.StartsWith("ui/"))
        {
            return AssetCategory.UI;
        }
        else if (this.m_RelativePath.StartsWith("audio/"))
        {
            return AssetCategory.Audio;
        }
        else if (this.m_RelativePath.StartsWith("mob/"))
        {
            return AssetCategory.Mob;
        }
        else if (this.m_RelativePath.StartsWith("maps/"))
        {
            return AssetCategory.Scene;
        }
        else if (this.m_RelativePath.StartsWith("effect/"))
        {
            return AssetCategory.Effect;
        }
        else if (this.m_RelativePath.StartsWith("config/"))
        {
            return AssetCategory.Config;
        }
        else if (this.m_RelativePath.StartsWith("graphic/"))
        {
            return AssetCategory.Shader;
        }
        else
        {
            return AssetCategory.Other;
        }
    }

    public bool CheckLocalFileValid()
    {
        if (this.extension == ".manifest" || this.extension == ".bytes" || this.extension == ".txt" || this.extension == ".dll")
        {
            var path = StringUtil.Contact(AssetPath.ExternalStorePath, this.m_RelativePath);
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists || fileInfo.Length != this.size || this.md5 != FileExtension.GetMD5HashFromFile(path))
            {
                return false;
            }
        }
        else if (string.IsNullOrEmpty(this.extension) || this.extension.Length == 0)
        {
            var path = StringUtil.Contact(AssetPath.ExternalStorePath, this.m_RelativePath);
            var fileInfo = new FileInfo(path);

            var manifestAssetVersion = AssetVersionUtility.GetAssetVersion(StringUtil.Contact(this.m_RelativePath, ".manifest"));
            if (!fileInfo.Exists || fileInfo.Length != this.size || manifestAssetVersion == null || !manifestAssetVersion.CheckLocalFileValid())
            {
                return false;
            }
        }
        else
        {
            var path = StringUtil.Contact(AssetPath.ExternalStorePath, this.m_RelativePath);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists || fileInfo.Length != this.size)
            {
                return false;
            }
        }

        return true;
    }

    public string GetAssetPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return StringUtil.Contact(AssetPath.ExternalStorePath, m_RelativePath);
            default:
                var path = StringUtil.Contact(AssetPath.ExternalStorePath, m_RelativePath);
                if (File.Exists(path))
                {
                    return path;
                }
                else
                {
                    var streamingAssetsPath = StringUtil.Contact(AssetPath.StreamingAssetPath, m_RelativePath);
                    if (File.Exists(streamingAssetsPath))
                    {
                        return streamingAssetsPath;
                    }
                    else
                    {
                        return path;
                    }
                }
        }
    }

    public enum StorageLocation
    {
        StreamingAsset,
        ExternalStore
    }

    public enum AssetCategory
    {
        UI,
        Audio,
        Mob,
        Scene,
        Effect,
        Config,
        Shader,
        Dll,
        Other,
    }

}
