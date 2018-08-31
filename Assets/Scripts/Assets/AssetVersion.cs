using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetVersion
{
    string m_RelativePath = string.Empty;
    public string relativePath { get { return m_RelativePath; } }

    string m_FileName;
    public string fileName { get { return m_FileName; } }

    string m_Extersion = string.Empty;
    public string extersion { get { return m_Extersion; } }

    string m_Md5 = string.Empty;
    public string md5 { get { return m_Md5; } }

    int m_Size = 0;
    public int size { get { return m_Size; } }       //字节     

    StorageLocation m_FileLocation = StorageLocation.ExternalStore;
    public StorageLocation fileLocation { get { return m_FileLocation; } }

    bool m_LocalValid = false;
    public bool localValid
    {
        get { return m_LocalValid; }
        set { m_LocalValid = value; }
    }

    public AssetVersion(string _versionString)
    {
        var strings = _versionString.Split('\t');

        m_RelativePath = strings[0];
        m_Extersion = strings[1];
        int.TryParse(strings[2], out m_Size);
        m_Md5 = strings[3];

        var paths = m_RelativePath.Split('/');

        var lastPath = paths[paths.Length - 1];
        var index = lastPath.IndexOf('.');
        if (index != -1)
        {
            m_FileName = lastPath.Substring(0, index);
        }
        else
        {
            m_FileName = lastPath;
        }
    }

    public AssetCategory GetAssetCategory()
    {
        if (extersion == ".dll")
        {
            return AssetCategory.Dll;
        }
        else if (m_RelativePath.StartsWith("ui/"))
        {
            return AssetCategory.UI;
        }
        else if (m_RelativePath.StartsWith("audio/"))
        {
            return AssetCategory.Audio;
        }
        else if (m_RelativePath.StartsWith("mob/"))
        {
            return AssetCategory.Mob;
        }
        else if (m_RelativePath.StartsWith("maps/"))
        {
            return AssetCategory.Scene;
        }
        else if (m_RelativePath.StartsWith("effect/"))
        {
            return AssetCategory.Effect;
        }
        else if (m_RelativePath.StartsWith("config/"))
        {
            return AssetCategory.Config;
        }
        else if (m_RelativePath.StartsWith("graphic/"))
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
        if (extersion == ".manifest" || extersion == ".bytes" || extersion == ".txt" || extersion == ".dll")
        {
            var path = StringUtil.Contact(AssetPath.ExternalStorePath, m_RelativePath);
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists || fileInfo.Length != size || md5 != FileExtersion.GetMD5HashFromFile(path))
            {
                return false;
            }
        }
        else if (string.IsNullOrEmpty(extersion) || extersion.Length == 0)
        {
            var path = StringUtil.Contact(AssetPath.ExternalStorePath, m_RelativePath);
            var fileInfo = new FileInfo(path);

            var manifestAssetVersion = AssetVersionUtility.GetAssetVersion(StringUtil.Contact(m_RelativePath, ".manifest"));
            if (!fileInfo.Exists || fileInfo.Length != size || manifestAssetVersion == null || !manifestAssetVersion.CheckLocalFileValid())
            {
                return false;
            }
        }
        else
        {
            var path = StringUtil.Contact(AssetPath.ExternalStorePath, m_RelativePath);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists || fileInfo.Length != size)
            {
                return false;
            }
        }

        return true;
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
