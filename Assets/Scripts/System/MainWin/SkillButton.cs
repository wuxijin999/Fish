﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : UIBase, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    [SerializeField] int m_Index = 0;

    PointerState m_PointerState = PointerState.Up;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_PointerState = PointerState.Down;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.m_PointerState == PointerState.Down)
        {
            CastSkill();
        }

        this.m_PointerState = PointerState.Up;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.m_PointerState = PointerState.Exit;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (SkillCast.Instance.IsCountDown(this.m_Index))
        {


        }
    }

    private void CastSkill()
    {
        SkillCast.Instance.CastSkill(this.m_Index);
    }

    public enum PointerState
    {
        Down,
        Up,
        Exit,
    }

}


