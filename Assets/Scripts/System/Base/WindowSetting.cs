using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WindowSetting : MonoBehaviour
{
    [SerializeField] int m_Id;
    public int id { get { return this.m_Id; } }

    [SerializeField] Tween m_Tween;
    public Tween tween { get { return m_Tween; } }

    [SerializeField] ButtonEx m_Close;
    public ButtonEx close { get { return m_Close; } }

    [SerializeField] RectTransform m_BackGround;
    public RectTransform backGround { get { return m_BackGround; } }

    [SerializeField] RectTransform m_Content;
    public RectTransform content { get { return m_Content; } }

}
