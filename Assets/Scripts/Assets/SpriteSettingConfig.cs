using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Config/SpriteSettingConfig")]
public class SpriteSettingConfig : ScriptableObject
{
    public SpriteSetting[] spriteSettings;

    [System.Serializable]
    public class SpriteSetting
    {
        public string folderName;
        public SpriteMeshType meshType = SpriteMeshType.Tight;
        public TextureImporterAlphaSource alphaSource = TextureImporterAlphaSource.FromInput;
        public PlatformSetting[] platformSettings;
    }

    [System.Serializable]
    public class PlatformSetting
    {
        public string name;
        public int maxTextureSize = 2048;
        public TextureImporterFormat textureFormat = TextureImporterFormat.RGBA32;
        public int compressionQuality = 1;
    }

}


public static class SpriteSettingUtility
{
    public static bool Equal(this SpriteSettingConfig.SpriteSetting _spriteSetting, TextureImporter _textureImporter)
    {
        if (_spriteSetting.alphaSource != _textureImporter.alphaSource)
        {
            return false;
        }

        foreach (var platformSetting in _spriteSetting.platformSettings)
        {
            var nowPlatformSetting = _textureImporter.GetPlatformTextureSettings(platformSetting.name);
            if (nowPlatformSetting == null)
            {
                continue;
            }

            if (platformSetting.maxTextureSize != nowPlatformSetting.maxTextureSize)
            {
                return false;
            }

            if (platformSetting.textureFormat != nowPlatformSetting.format)
            {
                return false;
            }

            if (platformSetting.compressionQuality != nowPlatformSetting.compressionQuality)
            {
                return false;
            }
        }

        return true;
    }

}