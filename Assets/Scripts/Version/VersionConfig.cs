using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Config/VersionConfig")]
public class VersionConfig : ScriptableObject
{

    [SerializeField] public string m_AppId = string.Empty;
    public string appId { get { return this.m_AppId; } }

    [SerializeField] VersionAuthority m_VersionAuthority;
    public VersionAuthority versionAuthority { get { return this.m_VersionAuthority; } }

    [SerializeField] public string m_Version;
    public string version { get { return m_Version; } }

    [SerializeField] int m_Branch = 0;
    public int branch { get { return this.m_Branch; } }

    [SerializeField] string m_ClientFlag;
    public string clientFlag { get { return this.m_ClientFlag; } }

    [SerializeField] InstalledAsset m_AssetAccess = InstalledAsset.IngoreDownLoad;
    public InstalledAsset assetAccess { get { return this.m_AssetAccess; } }

    [SerializeField] bool m_PartAssetPackage = false;
    public bool partAssetPackage { get { return this.m_PartAssetPackage; } }

    [SerializeField] string m_BuildTime;
    public string buildTime { get { return this.m_BuildTime; } }

    [SerializeField] int m_BuildIndex;
    public int buildIndex { get { return this.m_BuildIndex; } }

    public void Read(string _data)
    {
        var dataStrings = _data.Split('\t');
        this.m_AppId = dataStrings[1];
        this.m_VersionAuthority = (VersionAuthority)int.Parse(dataStrings[2]);
        this.m_Version = dataStrings[3];
        this.m_ClientFlag = dataStrings[4];
        this.m_Branch = int.Parse(dataStrings[5]);
        this.m_AssetAccess = (InstalledAsset)int.Parse(dataStrings[6]);
        this.m_PartAssetPackage = int.Parse(dataStrings[7]) == 1;
    }

    static VersionConfig config = null;
    public static VersionConfig Get()
    {
        if (config == null)
        {
            config = BuiltInAssets.LoadConfig<VersionConfig>("VersionConfig");
        }

        return config;
    }

    public static void Copy(VersionConfig _from, VersionConfig _to)
    {
        _to.m_VersionAuthority = _from.m_VersionAuthority;
        _to.m_Version = _from.m_Version;
        _to.m_AppId = _from.m_AppId;
        _to.m_Branch = _from.m_Branch;
        _to.m_AssetAccess = _from.m_AssetAccess;
        _to.m_PartAssetPackage = _from.m_PartAssetPackage;
        _to.m_BuildTime = _from.m_BuildTime;
        _to.m_ClientFlag = _from.m_ClientFlag;
    }
}

public enum InstalledAsset
{
    NullAsset = 0,
    HalfAsset = 1,
    FullAsset = 2,
    IngoreDownLoad = 3,
}

public enum VersionAuthority
{
    Debug = 0,
    Release = 1,
}
