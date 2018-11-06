using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder
{
    public static void Build(string output, string category, BuildAssetBundleOptions bundleOption, BuildTarget buildTarget)
    {
        var assetBundles = AssetDatabase.GetAllAssetBundleNames();

        var filtratedAssetBundles = new List<string>();
        for (int i = 0; i < assetBundles.Length; i++)
        {
            var bundleName = assetBundles[i];
            if (bundleName.StartsWith(category))
            {
                filtratedAssetBundles.Add(bundleName);
            }
        }

        var assets = new List<AssetBundleBuild>();
        for (int i = 0; i < filtratedAssetBundles.Count; i++)
        {
            var build = new AssetBundleBuild();
            build.assetBundleName = filtratedAssetBundles[i] + ".unity3d";
            build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(filtratedAssetBundles[i]);
            assets.Add(build);
        }

        var rootPath = StringUtil.Contact(output, Path.AltDirectorySeparatorChar, category);
        var mainFile = StringUtil.Contact(output, Path.AltDirectorySeparatorChar, GetMainFestFileName(buildTarget));
        var mainFileRename = StringUtil.Contact(output, Path.AltDirectorySeparatorChar, category, "_assetbundle");
        var manifest = StringUtil.Contact(output, Path.AltDirectorySeparatorChar, GetMainFestFileName(buildTarget), ".manifest");
        var manifestRename = StringUtil.Contact(output, Path.AltDirectorySeparatorChar, category, "_assetbundle.manifest");

        if (Directory.Exists(rootPath))
        {
            Directory.Delete(rootPath, true);
        }

        Directory.CreateDirectory(rootPath);

        if (File.Exists(mainFileRename))
        {
            File.Delete(mainFileRename);
        }

        if (File.Exists(manifestRename))
        {
            File.Delete(manifestRename);
        }

        BuildPipeline.BuildAssetBundles(output, assets.ToArray(), bundleOption, buildTarget);

        File.Move(mainFile, mainFileRename);
        File.Move(manifest, manifestRename);
    }

    static string GetMainFestFileName(BuildTarget _target)
    {
        switch (_target)
        {
            case BuildTarget.StandaloneWindows:
                return "standalone";
            case BuildTarget.Android:
                return "android";
            case BuildTarget.iOS:
                return "ios";
            default:
                return string.Empty;
        }
    }

}
