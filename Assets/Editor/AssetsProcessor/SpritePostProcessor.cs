using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpritePostProcessor : AssetPostprocessor
{

    void OnPostprocessSprites(Texture texture, Sprite[] sprites)
    {
        if (!this.assetPath.Contains(AssetPath.UI_SPRITE_ROOT_PATH))
        {
            return;
        }

        var importer = this.assetImporter as TextureImporter;
        var paths = this.assetPath.Split('/');
        var directroy = paths[paths.Length - 2];

        importer.assetBundleName = StringUtil.Contact("ui/sprite/", directroy).ToLower();
        importer.spritePackingTag = directroy.ToLower();
        importer.textureType = TextureImporterType.Sprite;
    }

}
