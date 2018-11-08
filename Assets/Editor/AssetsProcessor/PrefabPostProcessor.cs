using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabPostProcessor : AssetPostprocessor
{

    void OnPostprocessModel(GameObject prefab)
    {
        RemoveMissingScript(prefab);

        if (this.assetPath.Contains(AssetPath.UI_WINDOW_PATH))
        {
            this.assetImporter.assetBundleName = "ui/window";
        }
        else if (this.assetPath.Contains(AssetPath.UI_PREFAB_PATH))
        {
            this.assetImporter.assetBundleName = "ui/prefab";
        }
        else if (this.assetPath.Contains(AssetPath.EFFECT_ROOT_PATH))
        {
            var paths = this.assetPath.Split('/');
            var directroy = paths[paths.Length - 2];
            this.assetImporter.assetBundleName = StringUtil.Contact("effect/", directroy);
        }
        else if (this.assetPath.Contains(AssetPath.MOB_ROOT_PATH))
        {
            var paths = this.assetPath.Split('/');
            var directroy = paths[paths.Length - 2];
            this.assetImporter.assetBundleName = StringUtil.Contact("mob/", directroy);
        }
    }

    static void RemoveMissingScript(GameObject gameObject)
    {
        var so = new SerializedObject(gameObject);
        var soProperties = so.FindProperty("m_Component");
        var components = gameObject.GetComponents<Component>();
        var propertyIndex = 0;
        foreach (var component in components)
        {
            if (component == null)
            {
                soProperties.DeleteArrayElementAtIndex(propertyIndex);
            }
            ++propertyIndex;
        }
        so.ApplyModifiedProperties();
    }

}
