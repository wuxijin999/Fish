using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ButtonEx : Button
{

    [SerializeField] float m_Interval;
    [SerializeField] int m_PositiveSound = 0;
    [SerializeField] int m_NegativeSound = 0;
    [SerializeField] TextEx m_TitleMesh;
    [SerializeField] Color m_NormalColor = Color.white;
    [SerializeField] Color m_DisableColor = Color.white;
    [SerializeField] TextEx m_CoolDownText;
    [SerializeField] ImageEx m_Image;

    float coolDownTimer = 0f;

    float m_AbleTime = 0f;
    public float ableTime {
        get { return this.m_AbleTime; }
        private set { this.m_AbleTime = value; }
    }

    State m_State;
    public State state {
        get { return this.m_State; }
        private set {
            this.m_State = value;
        }
    }

    string m_Title;
    public string title {
        get { return this.m_Title; }
        set {
            if (this.m_Title != value)
            {
                this.m_Title = value;
                this.m_TitleMesh.text = this.m_Title;
            }
        }
    }

    public void SetState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.CoolDown:
                this.m_Image.gray = true;
                this.m_TitleMesh.color = m_DisableColor;
                DisplayCoolDown();
                break;
            case State.Disable:
                this.m_Image.gray = true;
                this.m_TitleMesh.color = m_DisableColor;
                break;
            case State.Normal:
                this.m_Image.gray = false;
                this.m_TitleMesh.color = m_NormalColor;
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
                if (this.m_Interval > 0.033f)
                {
                    this.ableTime = Time.realtimeSinceStartup + Mathf.Clamp(this.m_Interval, 0, float.MaxValue);
                    SetState(State.CoolDown);
                }
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetState(state);
    }

    private void PlayPositiveSound()
    {
    }

    private void PlayNegativeSound()
    {
    }

    private void LateUpdate()
    {
        if (this.state == State.CoolDown)
        {
            this.coolDownTimer += Time.deltaTime;

            if (this.coolDownTimer > 1f)
            {
                this.coolDownTimer = 0f;
                DisplayCoolDown();
            }

            if (Time.realtimeSinceStartup > this.ableTime)
            {
                SetState(State.Normal);
            }
        }
    }

    private void DisplayCoolDown()
    {
        var surplusTime = this.ableTime - Time.realtimeSinceStartup;
        if (surplusTime >= 1f)
        {
            this.m_TitleMesh.gameObject.SetActive(false);
            this.m_CoolDownText.gameObject.SetActive(true);
            this.m_CoolDownText.SetText(Mathf.RoundToInt(surplusTime));
        }
        else
        {
            this.m_TitleMesh.gameObject.SetActive(true);
            this.m_CoolDownText.gameObject.SetActive(false);
        }
    }

    public enum State
    {
        Normal = 0,
        CoolDown = 1,
        Disable = 2,
    }


}
