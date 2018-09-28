//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Tuesday, October 31, 2017
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;


public class FunctionButton : Button
{
    [SerializeField] int m_Order = 0;
    public int order {
        get { return m_Order; }
    }

    [SerializeField] int m_FunctionId = -1;
    public int functionId {
        get { return m_FunctionId; }
    }

    [SerializeField] int m_Audio = 1;
    [SerializeField] Button m_Button;
    [SerializeField] ImageEx m_Icon;
    [SerializeField] TextEx m_Title;
    [SerializeField] RectTransform m_Locked;
    [SerializeField] FunctionButtonGroup m_Group;
    public FunctionButtonGroup group {
        get { return m_Group; }
        set {
            if (m_Group != null)
            {
                m_Group.UnRegister(this);
            }
            m_Group = value;
            if (m_Group != null)
            {
                m_Group.Register(this);
            }
        }
    }


    State m_State = State.Normal;
    public State state {
        get { return m_State; }
        set {
            if (m_State != value)
            {
                m_State = value;
                OnStateChange();
            }
        }
    }

    protected override void Awake()
    {
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (group != null)
        {
            group.Register(this);
        }

        OnStateChange();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (group != null)
        {
            group.UnRegister(this);
        }
    }

    protected override void OnDestroy()
    {
    }

    public void Invoke(bool _force)
    {
        OnPointerClick(null);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        switch (m_State)
        {
            case State.Locked:
                break;
            case State.Normal:
                if (base.onClick != null)
                {
                    base.onClick.Invoke();
                    SoundUtil.Instance.PlaySound(m_Audio);
                }
                break;
            case State.Selected:
                break;
            default:
                break;
        }
    }

    private void OnStateChange()
    {
        if (m_Locked != null)
        {
            m_Locked.gameObject.SetActive(m_State == State.Locked);
        }

        if (m_Group != null && m_State == State.Selected)
        {
            m_Group.NotifyToggleOn(this);
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        OnStateChange();
    }
#endif

    public enum State
    {
        Locked,
        Normal,
        Selected,
    }
}




