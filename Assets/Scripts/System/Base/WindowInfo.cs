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
    public int depth
    {
        get
        {
            return this.m_Depth;
        }
        set
        {
            if (this.m_DynamicDepth)
            {
                this.m_Depth = value;
                this.m_Canvas.sortingOrder = this.m_Depth;
            }
        }
    }

    [SerializeField] bool m_Interactable = false;
    public bool interactable
    {
        get { return this.m_Interactable; }
        set
        {
            this.m_Interactable = value;
            this.m_Raycaster.enabled = this.m_Interactable;
        }
    }

    [SerializeField] bool m_ClickEmptyToClose;
    public bool clickEmptyToClose
    {
        get { return this.m_ClickEmptyToClose; }
    }

    [SerializeField] bool m_Persistent;
    public bool persistent { get { return this.m_Persistent; } }

    [SerializeField] bool m_FullScreen;
    public bool fullScreen { get { return this.m_FullScreen; } }

    [SerializeField] Tween m_Tween;
    public Tween tween { get { return this.m_Tween; } }

    private void Awake()
    {
        if (!this.m_DynamicDepth)
        {
            this.m_Canvas.sortingOrder = this.m_Depth;
        }

        this.m_Raycaster.enabled = this.m_Interactable;
    }

}
