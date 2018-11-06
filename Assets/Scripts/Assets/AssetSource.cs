using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSource
{

    public static bool allFromEditor {
        get {
            return audioFromEditor
                && effectFromEditor
                && mobFromEditor
                && refdataFromEditor
                && sceneFromEditor
                && shaderFromEditor
                && uiFromEditor
                && builtInFromEditor;
        }
        set {
            audioFromEditor = value;
            effectFromEditor = value;
            mobFromEditor = value;
            refdataFromEditor = value;
            shaderFromEditor = value;
            sceneFromEditor = value;
            uiFromEditor = value;
            builtInFromEditor = value;
        }
    }

    static bool m_AudioFromEditor = false;
    public static bool audioFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_AudioFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_AudioFromEditor", value);
#else
            m_AudioFromEditor = value;
#endif
        }
    }

    static bool m_EffectFromEditor = false;
    public static bool effectFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_EffectFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_EffectFromEditor", value);
#else
            m_AudioFromEditor = value;
#endif
        }
    }

    static bool m_MobFromEditor = false;
    public static bool mobFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_MobFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_MobFromEditor", value);
#else
            m_MobFromEditor = value;
#endif
        }
    }

    static bool m_RefdataFromEditor = false;
    public static bool refdataFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_RefdataFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_RefdataFromEditor", value);
#else
            m_RefdataFromEditor = value;
#endif
        }
    }

    static bool m_SceneFromEditor = false;
    public static bool sceneFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_SceneFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_SceneFromEditor", value);
#else
            m_SceneFromEditor = value;
#endif
        }
    }

    static bool m_ShaderFromEditor = false;
    public static bool shaderFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_ShaderFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_ShaderFromEditor", value);
#else
            m_ShaderFromEditor = value;
#endif
        }
    }

    static bool m_UIFromEditor = false;
    public static bool uiFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_UIFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_UIFromEditor", value);
#else
            m_UIFromEditor = value;
#endif
        }
    }

    static bool m_BuiltInFromEditor = false;
    public static bool builtInFromEditor {
        get {
#if UNITY_EDITOR
            return LocalSave.GetBool("Asset_BuiltInFromEditor", true);
#else
            return false;
#endif
        }
        set {
#if UNITY_EDITOR
            LocalSave.SetBool("Asset_BuiltInFromEditor", value);
#else
            m_BuiltInFromEditor = value;
#endif
        }
    }

}
