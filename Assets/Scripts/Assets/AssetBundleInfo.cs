using UnityEngine;

public struct AssetBundleInfo
{
    public string name;
    public Hash128 hash;
    public string[] dependentBundles;

    public AssetBundleInfo(string bundleName, Hash128 hash128, string[] dependentBundleArray)
    {
        this.name = bundleName;
        this.hash = hash128;
        this.dependentBundles = dependentBundleArray;
    }
}
