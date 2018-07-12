using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPath
{

    public const string AssetDependentFileBundleName =
#if UNITY_ANDROID
        "android";
#elif UNITY_IOS
        "ios";
#else
        "standalone";
#endif

    public readonly static string AssetRootPath = Application.dataPath + "/ResourcesOut";
    public const string AssetRootRelativePath = "Assets/ResourcesOut";
    public const string AssetDependentFileAssetName = "AssetBundleManifest";

    public static readonly string StreamingAssetPath = Application.streamingAssetsPath + "/" + AssetDependentFileBundleName;
    public static readonly string ExternalStorePath = Application.persistentDataPath;

    #region 具体asset资源读取路径

    public static readonly string UI_ROOT_PATH = AssetRootPath + "/1_UI";
    public static readonly string UI_PREFAB_PATH = UI_ROOT_PATH + "/Prefab";
    public static readonly string UI_WINDOW_PATH = UI_ROOT_PATH + "/Window";
    public static readonly string UI_SPRITE_ROOT_PATH = UI_ROOT_PATH + "/Sprite";

    public static readonly string MOB_ROOT_PATH = AssetRootPath + "/2_Mob";
    public static readonly string SHADER_ROOT_PATH = AssetRootPath + "/3_Shader";
    public static readonly string SCENE_ROOT_PATH = AssetRootPath + "/4_Scenes";
    public static readonly string CONFIG_ROOT_PATH = AssetRootPath + "/5_Config";
    public static readonly string AUDIO_ROOT_PATH = AssetRootPath + "/6_Audio";

    #endregion

}
