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
        this.m_Slider.value = this.value;
        this.refspeed = 0f;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (this.m_Slider != null && Mathf.Abs(this.m_Slider.value - this.value) > 0.001f)
        {
            this.m_Slider.value = Mathf.SmoothDamp(this.m_Slider.value, this.value, ref this.refspeed, this.m_Smooth);

            if (this.m_Progess != null)
            {
                switch (this.m_Pattern)
                {
                    case DigitPattern.Integet:
                        this.m_Progess.text = Mathf.RoundToInt(this.m_Slider.value).ToString();
                        break;
                    case DigitPattern.Percentage:
                        this.m_Progess.text = string.Concat(Mathf.RoundToInt(this.m_Slider.value * 100).ToString(), "%");
                        break;
                    default:
                        this.m_Progess.text = this.m_Slider.value.ToString("f2");
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



