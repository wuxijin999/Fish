using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class AssetVersionUtility
{

    public static bool hasDownLoadFullAsset {
        get { return LocalSave.GetBool("HasDownLoadFullAsset"); }
        set { LocalSave.SetBool("HasDownLoadFullAsset", value); }
    }

    static bool m_PriorAssetDownLoadDone = false;
    public static bool priorAssetDownLoadDone {
        get {
            switch (VersionConfig.Get().assetAccess)
            {
                case InstalledAsset.NullAsset:
                case InstalledAsset.HalfAsset:
                case InstalledAsset.FullAsset:
                    return m_PriorAssetDownLoadDone;
                case InstalledAsset.IngoreDownLoad:
                    return true;
                default:
                    return true;
            }
        }
    }

    static bool m_UnPriorAssetDownLoadDone = false;
    public static bool unPriorAssetDownLoadDone {
        get {
            switch (VersionConfig.Get().assetAccess)
            {
                case InstalledAsset.NullAsset:
                case InstalledAsset.HalfAsset:
                case InstalledAsset.FullAsset:
                    return m_UnPriorAssetDownLoadDone;
                case InstalledAsset.IngoreDownLoad:
                    return true;
                default:
                    return true;
            }
        }
    }

    static Dictionary<string, AssetVersion> assetVersions = new Dictionary<string, AssetVersion>();
    static List<AssetVersion> priorDownLoadAssetVersions = new List<AssetVersion>();
    static List<AssetVersion> unpriorDownLoadAssetVersions = new List<AssetVersion>();

    public static void GetAssetVersionFile()
    {
        Debug.LogFormat("开始获取资源版本文件：时间 {0}", DateTime.Now);
        var assetVersionUrl = StringUtility.Contact(VersionUtility.Instance.versionInfo.GetResourcesURL(VersionConfig.Get().branch), "/", "AssetsVersion.txt");
        HttpRequest.Instance.RequestHttpGet(assetVersionUrl, HttpRequest.defaultHttpContentType, 3, OnGetAssetVersionFile);
    }

    private static void OnGetAssetVersionFile(bool _ok, string _result)
    {
        Debug.LogFormat("获取资源版本文件结果：时间 {0}，结果 {1}", DateTime.Now, _ok);

        if (_ok)
        {
            var assetVersions = AssetVersionUtility.UpdateAssetVersions(_result);
            foreach (var assetVersion in assetVersions.Values)
            {
                if (!assetVersion.CheckLocalFileValid())
                {
                    priorDownLoadAssetVersions.Add(assetVersion);
                    assetVersion.localValid = false;
                }
                else
                {
                    assetVersion.localValid = true;
                }
            }

            m_PriorAssetDownLoadDone = priorDownLoadAssetVersions.Count <= 0;
            m_UnPriorAssetDownLoadDone = unpriorDownLoadAssetVersions.Count <= 0;

            if (!m_PriorAssetDownLoadDone)
            {
                BeginDownLoad(true);
            }
        }
        else
        {
            Clock.Create(DateTime.Now + new TimeSpan(TimeSpan.TicksPerSecond), GetAssetVersionFile);
        }
    }

    public static void BeginDownLoad(bool _prior)
    {
        if (_prior)
        {
            m_PriorAssetDownLoadDone = false;
            DownLoadAndDiscompressTask.Instance.Prepare(priorDownLoadAssetVersions, true, () => { m_PriorAssetDownLoadDone = true; });
        }
        else
        {
            m_UnPriorAssetDownLoadDone = false;
            DownLoadAndDiscompressTask.Instance.Prepare(unpriorDownLoadAssetVersions, false, () =>
            {
                m_UnPriorAssetDownLoadDone = true;
                hasDownLoadFullAsset = true;
            });
        }
    }

    public static Dictionary<string, AssetVersion> UpdateAssetVersions(string _assetVersionFile)
    {
        var lines = _assetVersionFile.Split(new string[] { FileExtersion.lineSplit }, StringSplitOptions.RemoveEmptyEntries);
        assetVersions.Clear();
        for (int i = 0; i < lines.Length; i++)
        {
            var assetVersion = new AssetVersion(lines[i]);
            assetVersions[assetVersion.relativePath] = assetVersion;
        }

        return assetVersions;
    }

    public static AssetVersion GetAssetVersion(string _assetBundleName)
    {
        if (assetVersions.ContainsKey(_assetBundleName))
        {
            return assetVersions[_assetBundleName];
        }
        else
        {
            return null;
        }
    }

    public static string GetAssetFilePath(string _assetKey)
    {
        var path = string.Empty;
        if (assetVersions.ContainsKey(_assetKey))
        {
            var assetVersion = assetVersions[_assetKey];
            switch (assetVersion.fileLocation)
            {
                case AssetVersion.StorageLocation.StreamingAsset:
                    path = StringUtility.Contact(AssetPath.StreamingAssetPath, _assetKey);
                    break;
                case AssetVersion.StorageLocation.ExternalStore:
                    path = StringUtility.Contact(AssetPath.ExternalStorePath, _assetKey);
                    break;
            }
        }
        else
        {
            path = StringUtility.Contact(AssetPath.StreamingAssetPath, _assetKey);
        }

        return path;
    }

}
