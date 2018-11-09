using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabPostProcessor : AssetPostprocessor
{

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var assetPath in importedAssets)
        {
            if (assetPath.EndsWith(".prefab"))
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab == null)
                {
                    continue;
                }

                RemoveMissingScript(prefab);
                var assetImporter = AssetImporter.GetAtPath(assetPath);
                if (assetPath.Contains(AssetPath.UI_WINDOW_PATH))
                {
                    assetImporter.assetBundleName = "ui/window";
                }
                else if (assetPath.Contains(AssetPath.UI_PREFAB_PATH))
                {
                    assetImporter.assetBundleName = "ui/prefab";
                }
                else if (assetPath.Contains(AssetPath.EFFECT_ROOT_PATH))
                {
                    var paths = assetPath.Split('/');
                    var directroy = paths[paths.Length - 2];
                    assetImporter.assetBundleName = StringUtil.Contact("effect/", directroy);
                }
                else if (assetPath.Contains(AssetPath.MOB_ROOT_PATH))
                {
                    var paths = assetPath.Split('/');
                    var directroy = paths[paths.Length - 2];
                    assetImporter.assetBundleName = StringUtil.Contact("mob/", directroy);
                }
            }
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
