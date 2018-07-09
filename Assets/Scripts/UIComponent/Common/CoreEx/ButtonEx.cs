using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonEx : Button
{

    public event Action ableTimeChangeEvent;

    public float interval;
    public bool customPositiveSound = false;
    public bool customNegativeSound = false;
    public int positiveSound = 0;
    public int negativeSound = 0;

    float m_AbleTime = 0f;
    public float ableTime
    {
        get { return m_AbleTime; }
        private set
        {
            m_AbleTime = value;
            if (ableTimeChangeEvent != null)
            {
                ableTimeChangeEvent();
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (Time.realtimeSinceStartup < ableTime)
        {
            PlayNegativeSound();
            return;
        }

        base.OnPointerClick(eventData);

        PlayPositiveSound();
        ableTime = Time.realtimeSinceStartup + Mathf.Clamp(interval, 0, float.MaxValue);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void PlayPositiveSound()
    {
        if (customPositiveSound)
        {
            //   SoundPlayer.Instance.PlayUIAudio(positiveSound);
        }
        else
        {
            // SoundPlayer.Instance.PlayUIAudio(SoundPlayer.defaultClickPositiveAudio);
        }
    }

    private void PlayNegativeSound()
    {
        if (customNegativeSound)
        {
            // SoundPlayer.Instance.PlayUIAudio(negativeSound);
        }
        else
        {
            //SoundPlayer.Instance.PlayUIAudio(SoundPlayer.defaultClickNegativeAudio);
        }
    }

}
