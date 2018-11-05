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

    private void Awake()
    {
        if (m_BackGround == null)
        {
            var temp = this.transform.Find("BackGround");
            if (temp != null)
            {
                m_BackGround = temp as RectTransform;
            }
        }

        if (m_Content == null)
        {
            var temp = this.transform.Find("Content");
            if (temp != null)
            {
                m_Content = temp as RectTransform;
            }
        }

    }

}
