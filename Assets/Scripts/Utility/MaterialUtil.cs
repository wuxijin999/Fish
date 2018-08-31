using UnityEngine;
using System;

public static class MaterialUtil
{

    public static Material GetDefaultSpriteGrayMaterial()
    {
        return Resources.Load<Material>("Material/SpriteGray");
    }

    public static Material GetInstantiatedSpriteGrayMaterial()
    {
        var material = new Material(GetDefaultSpriteGrayMaterial());
        return material;
    }

    public static Material GetSmoothMaskGrayMaterial()
    {
        return Resources.Load<Material>("Material/SmoothMaskGray");
    }

    public static Material GetInstantiatedSpriteTwinkleMaterial()
    {
        var material = Resources.Load<Material>("Material/Flash");
        return new Material(material);
    }

    public static Material GetUIDefaultGraphicMaterial()
    {
        return UnityEngine.UI.Image.defaultGraphicMaterial;
    }

    public static Material GetUIBlurMaterial()
    {
        return Resources.Load<Material>("Material/GUIBlurMaterial");
    }

    public static Material GetGUIRenderTextureMaterial()
    {
        return Resources.Load<Material>("Material/UI_RenderTexture");
    }

    public static void SetRenderSortingOrder(this GameObject root, int sortingOrder, bool includeChildren)
    {

        if (root == null)
        {
            throw new NullReferenceException();
        }

        if (includeChildren)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                renderer.sortingOrder = sortingOrder;
            }
        }
        else
        {
            var renderer = root.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = sortingOrder;
            }
        }

    }


    static Material m_HudMaterial;
    public static Material hudMaterial
    {
        get
        {
            if (m_HudMaterial == null)
            {
                m_HudMaterial = Resources.Load<Material>("Material/HUD_HeadupName");
                m_HudMaterial.SetTexture("_Tex1", FontUtil.preferred.material.mainTexture);
            }
            return m_HudMaterial;
        }
    }


    static Shader m_HeroShader;
    static Shader m_PlayerShader;
    static Shader heroShader { get { return m_HeroShader ?? (m_HeroShader = Shader.Find("Character/Character, Emission,Flow_Hero")); } }
    static Shader playerShader { get { return m_PlayerShader ?? (m_PlayerShader = Shader.Find("Character/Character, Emission,Flow")); } }

    public static void SwitchXrayShader(Material _material, bool _isHero)
    {
        //         if (_material==null)
        //         {
        //             return;
        //         }
        // 
        //         if (_isHero)
        //         {
        //             if (_material.shader.name == "Character/Character, Emission,Flow")
        //             {
        //                 _material.shader = heroShader;
        //                 _material.SetColor("_XRayColor", new Color32(0, 107, 255, 255));
        //             }
        //         }
        //         else
        //         {
        //             if (_material.shader.name == "Character/Character, Emission,Flow_Hero")
        //             {
        //                 _material.shader = playerShader;
        //             }
        //         }
    }

}
