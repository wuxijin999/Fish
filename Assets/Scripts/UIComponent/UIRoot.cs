using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{

    public static Camera uiCamera { get; private set; }
    public static RectTransform windowRoot { get; private set; }
    public static HUDRoot hudRoot { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    static void RunTimeInit()
    {
        if (GameObject.FindObjectOfType<UIRoot>() == null)
        {
            var prefab = Resources.Load<GameObject>("UIPrefab/UIRoot");
            var instance = GameObject.Instantiate(prefab);
            instance.name = "UIRoot";
            DontDestroyOnLoad(instance);
        }
    }

    [SerializeField] Camera m_UICamera;
    [SerializeField] HUDRoot m_HUDRoot;
    [SerializeField] RectTransform m_WindowRoot;

    private void Awake()
    {
        if (this.m_UICamera != null)
        {
            uiCamera = this.m_UICamera;
        }

        if (this.m_WindowRoot != null)
        {
            windowRoot = this.m_WindowRoot;
        }

        if (this.m_HUDRoot != null)
        {
            hudRoot = this.m_HUDRoot;
        }
    }

}
