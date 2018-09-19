using UnityEngine;

public class AssetBundleInfo {

    public string name = null;
    public Hash128 hash;
    public string[] dependentBundles = null;

    public AssetBundleInfo(string bundleName, Hash128 hash128, string[] dependentBundleArray) {
        this.name = bundleName;
        this.hash = hash128;
        this.dependentBundles = dependentBundleArray;
    }
}
