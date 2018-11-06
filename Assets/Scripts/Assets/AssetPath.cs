using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPath
{
    public readonly static string AssetRootPath = Application.dataPath + "/ResourcesOut/";
    public const string AssetRootRelativePath = "Assets/ResourcesOut/";
    public const string AssetDependentFileAssetName = "AssetBundleManifest";

    public static readonly string StreamingAssetPath = Application.streamingAssetsPath + "/";
    public static readonly string ExternalStorePath = Application.persistentDataPath + "/";

    #region 具体asset资源读取路径

    public static readonly string UI_ROOT_PATH = AssetRootRelativePath + "1_UI/";
    public static readonly string UI_PREFAB_PATH = UI_ROOT_PATH + "Prefab/";
    public static readonly string UI_WINDOW_PATH = UI_ROOT_PATH + "Window/";
    public static readonly string UI_SPRITE_ROOT_PATH = UI_ROOT_PATH + "Sprite/";

    public static readonly string MOB_ROOT_PATH = AssetRootRelativePath + "2_Mob/";
    public static readonly string SHADER_ROOT_PATH = AssetRootRelativePath + "3_Shader/";
    public static readonly string SCENE_ROOT_PATH = AssetRootRelativePath + "4_Scene/";
    public static readonly string CONFIG_ROOT_PATH = AssetRootRelativePath + "5_Config/";
    public static readonly string AUDIO_ROOT_PATH = AssetRootRelativePath + "6_Audio/";
    public static readonly string EFFECT_ROOT_PATH = AssetRootRelativePath + "7_Effect/";
    public static readonly string BUILTIN_ROOT_PATH = AssetRootRelativePath + "8_BuiltIn/";

    #endregion

}
