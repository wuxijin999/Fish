using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{

    public static Camera uiCamera { get; private set; }
    public static RectTransform windowRoot { get; private set; }

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
    [SerializeField] RectTransform m_WindowRoot;

    private void Awake()
    {
        if (m_UICamera != null)
        {
            uiCamera = m_UICamera;
        }

        if (m_WindowRoot != null)
        {
            windowRoot = m_WindowRoot;
        }
    }

}
