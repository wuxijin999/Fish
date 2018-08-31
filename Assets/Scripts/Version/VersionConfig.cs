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
    public const string VERSION_ALTERNATIVE = "0.0.0";

    [SerializeField] public string m_AppId = string.Empty;
    public string appId { get { return m_AppId; } }

    [SerializeField] VersionAuthority m_VersionAuthority;
    public VersionAuthority versionAuthority { get { return m_VersionAuthority; } }

    [SerializeField] public string m_Version;
    public string version { get { return VersionCompare(m_Version, VERSION_ALTERNATIVE); } }

    [SerializeField] string m_ClientPackageFlag;
    public string clientPackageFlag { get { return m_ClientPackageFlag; } }

    [SerializeField] int m_Branch = 0;
    public int branch { get { return m_Branch; } }

    [SerializeField] InstalledAsset m_AssetAccess = InstalledAsset.IngoreDownLoad;
    public InstalledAsset assetAccess { get { return m_AssetAccess; } }

    [SerializeField] bool m_PartAssetPackage = false;
    public bool partAssetPackage { get { return m_PartAssetPackage; } }

    [SerializeField] string m_ProductName = string.Empty;
    public string productName { get { return m_ProductName; } }

    [SerializeField] string m_BundleIdentifier = string.Empty;
    public string bundleIdentifier { get { return m_BundleIdentifier; } }

    [SerializeField] string m_KeystoreFileName;
    public string keystoreFileName { get { return m_KeystoreFileName; } }

    [SerializeField] string m_KeystorePassword;
    public string keystorePassword { get { return m_KeystorePassword; } }

    [SerializeField] string m_KeystoreAlias;
    public string keystoreAlias { get { return m_KeystoreAlias; } }

    [SerializeField] string m_KeystoreAliasPassword;
    public string keystoreAliasPassword { get { return m_KeystoreAliasPassword; } }

    [SerializeField] string m_AppleDeveloperTeamID;
    public string appleDeveloperTeamID { get { return m_AppleDeveloperTeamID; } }

    [SerializeField] bool m_DebugVersion = false;
    public bool debugVersion
    {
        get { return m_DebugVersion; }
        set { m_DebugVersion = value; }
    }

    [SerializeField] bool m_IsBanShu = false;
    public bool isBanShu
    {
        get { return m_IsBanShu; }
        set { m_IsBanShu = value; }
    }

    [SerializeField] string m_BuildTime;
    public string buildTime
    {
        get { return m_BuildTime; }
        set { m_BuildTime = value; }
    }

    [SerializeField] int m_BuildIndex;
    public int buildIndex
    {
        get { return m_BuildIndex; }
        set { m_BuildIndex = value; }
    }

    public void Read(string _data)
    {
        var dataStrings = _data.Split('\t');
        m_AppId = dataStrings[1];
        m_VersionAuthority = (VersionAuthority)int.Parse(dataStrings[2]);
        m_Version = dataStrings[3];
        m_ClientPackageFlag = dataStrings[4];
        m_Branch = int.Parse(dataStrings[5]);
        m_AssetAccess = (InstalledAsset)int.Parse(dataStrings[6]);
        m_PartAssetPackage = int.Parse(dataStrings[7]) == 1;
        m_ProductName = dataStrings[8];
        m_BundleIdentifier = dataStrings[9];
        m_KeystoreFileName = dataStrings[10];
        m_KeystorePassword = dataStrings[11];
        m_KeystoreAlias = dataStrings[12];
        m_KeystoreAliasPassword = dataStrings[13];
        m_AppleDeveloperTeamID = dataStrings[14];
        m_DebugVersion = int.Parse(dataStrings[15]) == 1;
        m_IsBanShu = int.Parse(dataStrings[16]) == 1;
    }

#if UNITY_EDITOR
    [ContextMenu("Apply")]
    public void Apply()
    {
        var newVersionConfigPath = StringUtil.Contact("Assets/Resources/ScriptableObject/Config/VersionConfig", ".asset");

        var fromVersionConfig = this;
        var newVersionConfig = ScriptableObject.CreateInstance<VersionConfig>();
        if (File.Exists(newVersionConfigPath))
        {
            AssetDatabase.DeleteAsset(newVersionConfigPath);
        }

        Copy(fromVersionConfig, newVersionConfig);
        AssetDatabase.CreateAsset(newVersionConfig, newVersionConfigPath);
        EditorUtility.SetDirty(newVersionConfig);
        AssetDatabase.SaveAssets();
    }
#endif

    static VersionConfig config = null;
    public static VersionConfig Get()
    {
        if (config == null)
        {
            config = Resources.Load<VersionConfig>("ScriptableObject/Config/VersionConfig");
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
        _to.m_DebugVersion = _from.m_DebugVersion;
        _to.m_ProductName = _from.m_ProductName;
        _to.m_BundleIdentifier = _from.m_BundleIdentifier;
        _to.m_KeystoreFileName = _from.m_KeystoreFileName;
        _to.m_KeystoreAlias = _from.m_KeystoreAlias;
        _to.m_KeystorePassword = _from.m_KeystorePassword;
        _to.m_KeystoreAliasPassword = _from.m_KeystoreAliasPassword;
        _to.m_AppleDeveloperTeamID = _from.m_AppleDeveloperTeamID;
        _to.m_IsBanShu = _from.m_IsBanShu;
        _to.m_ClientPackageFlag = _from.m_ClientPackageFlag;
    }

    /// <summary>
    /// 比较两个版本，返回更大的那个
    /// </summary>
    /// <param name="_lhs"></param>
    /// <param name="_rhs"></param>
    /// <returns></returns>
    static string VersionCompare(string _lhs, string _rhs)
    {
        var lhsStrings = _lhs.Split('.');
        var rhsStrings = _rhs.Split('.');

        if (lhsStrings.Length > rhsStrings.Length)
        {
            return _lhs;
        }
        else if (lhsStrings.Length < rhsStrings.Length)
        {
            return _rhs;
        }
        else
        {
            var version1 = 0;
            for (int i = 0; i < lhsStrings.Length; i++)
            {
                var input = lhsStrings[i];
                var intTemp = 0;
                int.TryParse(input, out intTemp);
                version1 += intTemp * MathUtil.Power(100, lhsStrings.Length - i);
            }

            var version2 = 0;
            for (int i = 0; i < rhsStrings.Length; i++)
            {
                var input = rhsStrings[i];
                var intTemp = 0;
                int.TryParse(input, out intTemp);
                version2 += intTemp * MathUtil.Power(100, rhsStrings.Length - i);
            }

            return version1 > version2 ? _lhs : _rhs;

        }
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
    InterTest = 0,
    Release = 1,
}
