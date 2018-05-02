using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : SingletonMonobehaviour<UIRoot>
{

    [SerializeField] Camera m_UICamera;
    public Camera uiCamera { get { return m_UICamera; } }

    [SerializeField] RectTransform m_WindowRoot;
    public RectTransform windowRoot { get { return m_WindowRoot; } }


}
