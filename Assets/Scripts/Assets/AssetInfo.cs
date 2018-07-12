
public class AssetInfo {

    public string assetBundleName;
    public string name;
    public string path;

    public AssetInfo(string assetPath, string bundleName, string assetName) {
        path = assetPath;
        assetBundleName = bundleName;
        name = assetName;
    }

    public AssetInfo(string bundleName, string assetName) {
        assetBundleName = bundleName;
        name = assetName;
    }
}
