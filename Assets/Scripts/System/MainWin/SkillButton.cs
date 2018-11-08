//--------------------------------------------------------
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

    [SerializeField] ImageEx m_Icon;
    [SerializeField] RectTransform m_CoolDownContainer;
    [SerializeField] ImageEx m_CoolDownCircle;
    [SerializeField] TextEx m_CoolDownNumber;

    PointerState m_PointerState = PointerState.Up;

    public void Display(int skillId)
    {
        var config = SkillConfig.Get(skillId);
        m_Icon.SetSprite(config.icon);
    }

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

    private void OnEnable()
    {
        DisplayCountDown();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        DisplayCountDown();
    }

    private void DisplayCountDown()
    {
        if (SkillCast.Instance.IsCountDown(this.m_Index))
        {
            m_CoolDownContainer.SetActive(true);
            m_CoolDownNumber.SetText(SkillCast.Instance.GetSkillCountDown(this.m_Index));

            var amount = SkillCast.Instance.GetSkillCountDownAmount(this.m_Index);
            m_CoolDownCircle.fillAmount = amount;
        }
        else
        {
            m_CoolDownContainer.SetActive(false);
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



