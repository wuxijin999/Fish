
public class AssetInfo {

    public string assetBundleName;
    public string name;
    public string path;

    public AssetInfo(string assetPath, string bundleName, string assetName) {
        this.path = assetPath;
        this.assetBundleName = bundleName;
        this.name = assetName;
    }

    public AssetInfo(string bundleName, string assetName) {
        this.assetBundleName = bundleName;
        this.name = assetName;
    }
}
