using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ButtonEx : Button
{

    [SerializeField] float interval;
    [SerializeField] int positiveSound = 0;
    [SerializeField] int negativeSound = 0;
    [SerializeField] TextMeshProUGUI m_TitleMesh;
    [SerializeField] Color m_NormalColor;
    [SerializeField] Color m_DisableColor;
    [SerializeField] TextMeshProUGUI m_CoolDownText;
    [SerializeField] ImageEx m_Image;


    float coolDownTimer = 0f;

    float m_AbleTime = 0f;
    public float ableTime
    {
        get { return m_AbleTime; }
        private set { m_AbleTime = value; }
    }

    string m_Title;
    public string title
    {
        get { return m_Title; }
        set
        {
            if (m_Title != value)
            {
                m_Title = value;
                m_TitleMesh.text = m_Title;
            }
        }
    }

    State m_State;
    public State state
    {
        get { return m_State; }
        set
        {
            m_State = value;
        }
    }

    public void SetState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.CoolDown:
                m_Image.gray = true;
                break;
            case State.Disable:
                m_Image.gray = true;
                break;
            case State.Normal:
                m_Image.gray = false;
                break;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        switch (this.state)
        {
            case State.CoolDown:
                PlayNegativeSound();
                return;
            case State.Disable:
                return;
            case State.Normal:
                base.OnPointerClick(eventData);
                PlayPositiveSound();
                ableTime = Time.realtimeSinceStartup + Mathf.Clamp(interval, 0, float.MaxValue);
                SetState(State.CoolDown);
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void PlayPositiveSound()
    {
    }

    private void PlayNegativeSound()
    {
    }

    private void LateUpdate()
    {
        if (state == State.CoolDown)
        {
            coolDownTimer += Time.deltaTime;

            if (coolDownTimer > 1f)
            {
                coolDownTimer = 0f;
                DisplayCoolDown();
            }
        }

    }

    private void DisplayCoolDown()
    {
        var surplusTime = ableTime - Time.realtimeSinceStartup;
        if (surplusTime >= 1f)
        {
            m_TitleMesh.gameObject.SetActive(false);
            m_CoolDownText.gameObject.SetActive(true);
            m_CoolDownText.text = "";
        }
        else
        {
            m_TitleMesh.gameObject.SetActive(true);
            m_CoolDownText.gameObject.SetActive(false);
        }
    }

    public enum State
    {
        Normal = 0,
        CoolDown = 1,
        Disable = 2,
    }


}
