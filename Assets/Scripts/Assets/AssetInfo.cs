
public struct AssetInfo
{
    public string assetBundleName;
    public string name;

    public AssetInfo(string bundleName, string assetName)
    {
        this.assetBundleName = bundleName;
        this.name = assetName;
    }
}
