using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWidget : Widget
{
    [SerializeField] AttackButton m_AttackButton;
    [SerializeField] SkillButton m_Skill1;
    [SerializeField] SkillButton m_Skill2;
    [SerializeField] SkillButton m_Skill3;
    [SerializeField] SkillButton m_Skill4;
    [SerializeField] SkillButton m_SkillSpecial;

    protected override void OnBeforeActived()
    {
        base.OnBeforeActived();

        var skillId1 = SkillCast.Instance.skill1.Fetch();
        UpdateSkillButtonState(m_Skill1, skillId1);

        var skillId2 = SkillCast.Instance.skill2.Fetch();
        UpdateSkillButtonState(m_Skill2, skillId2);

        var skillId3 = SkillCast.Instance.skill3.Fetch();
        UpdateSkillButtonState(m_Skill3, skillId3);

        var skillId4 = SkillCast.Instance.skill4.Fetch();
        UpdateSkillButtonState(m_Skill4, skillId4);

        var skillSpecialId = SkillCast.Instance.skillSpecial.Fetch();
        UpdateSkillButtonState(m_SkillSpecial, skillSpecialId);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (SkillCast.Instance.skill1.dirty)
        {
            var skillId = SkillCast.Instance.skill1.Fetch();
            UpdateSkillButtonState(m_Skill1, skillId);
        }

        if (SkillCast.Instance.skill2.dirty)
        {
            var skillId = SkillCast.Instance.skill2.Fetch();
            UpdateSkillButtonState(m_Skill2, skillId);
        }

        if (SkillCast.Instance.skill3.dirty)
        {
            var skillId = SkillCast.Instance.skill3.Fetch();
            UpdateSkillButtonState(m_Skill3, skillId);
        }

        if (SkillCast.Instance.skill4.dirty)
        {
            var skillId = SkillCast.Instance.skill4.Fetch();
            UpdateSkillButtonState(m_Skill4, skillId);
        }

        if (SkillCast.Instance.skillSpecial.dirty)
        {
            var skillId = SkillCast.Instance.skillSpecial.Fetch();
            UpdateSkillButtonState(m_SkillSpecial, skillId);
        }
    }

    private void UpdateSkillButtonState(SkillButton skillButton, int skillId)
    {
        skillButton.SetActive(skillId != 0);
        if (skillId != 0)
        {
            skillButton.Display(skillId);
        }
    }

}
