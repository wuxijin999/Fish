//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmoothSlider : UIBase
{

    [SerializeField] Slider m_Slider;
    [SerializeField] TextEx m_Progess;
    [SerializeField] DigitPattern m_Pattern;
    [SerializeField] float m_Value;
    public float value {
        get { return this.m_Value; }
        set {
            this.m_Value = value;
            this.refspeed = 0f;
        }
    }

    [SerializeField] [Range(0.1f, 1f)] float m_Smooth = 0.2f;
    [SerializeField] bool m_Decimal = false;

    float refspeed = 0f;

    public void Reset()
    {
        m_Slider.value = value;
        refspeed = 0f;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (m_Slider != null && Mathf.Abs(m_Slider.value - value) > 0.001f)
        {
            m_Slider.value = Mathf.SmoothDamp(m_Slider.value, value, ref refspeed, m_Smooth);

            if (m_Progess != null)
            {
                switch (m_Pattern)
                {
                    case DigitPattern.Integet:
                        m_Progess.text = Mathf.RoundToInt(m_Slider.value).ToString();
                        break;
                    case DigitPattern.Percentage:
                        m_Progess.text = string.Concat(Mathf.RoundToInt(m_Slider.value * 100).ToString(), "%");
                        break;
                    default:
                        m_Progess.text = m_Slider.value.ToString("f2");
                        break;
                }
            }
        }
    }


    public enum DigitPattern
    {
        Normal,
        Percentage,
        Integet,
    }

}



