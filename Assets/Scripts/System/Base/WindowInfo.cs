using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowInfo : MonoBehaviour
{

    [SerializeField] Canvas m_Canvas;
    [SerializeField] GraphicRaycaster m_Raycaster;

    [SerializeField] bool m_DynamicDepth = false;
    [SerializeField] int m_Depth;
    public int depth {
        get {
            return m_Depth;
        }
        set {
            if (m_DynamicDepth)
            {
                m_Depth = value;
                m_Canvas.sortingOrder = m_Depth;
            }
        }
    }

    [SerializeField] bool m_Interactable = false;
    public bool interactable {
        get { return m_Interactable; }
        set {
            m_Interactable = value;
            m_Raycaster.enabled = m_Interactable;
        }
    }

    [SerializeField] bool m_ClickEmptyToClose;
    public bool clickEmptyToClose {
        get { return m_ClickEmptyToClose; }
    }

    [SerializeField] bool m_Persistent;
    public bool persistent { get { return m_Persistent; } }

    [SerializeField] bool m_FullScreen;
    public bool fullScreen { get { return m_FullScreen; } }

    [SerializeField] Tween m_Tween;
    public Tween tween { get { return m_Tween; } }

    private void Awake()
    {
        if (!m_DynamicDepth)
        {
            m_Canvas.sortingOrder = m_Depth;
        }

        m_Raycaster.enabled = m_Interactable;
    }

}
