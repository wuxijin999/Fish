//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillCast : Presenter<SkillCast>
{

    SkillModel model = new SkillModel();

    public readonly IntProperty skill1 = new IntProperty();
    public readonly IntProperty skill2 = new IntProperty();
    public readonly IntProperty skill3 = new IntProperty();
    public readonly IntProperty skill4 = new IntProperty();
    public readonly IntProperty skillSpecial = new IntProperty();

    public void SetSkill(int index, int skillId)
    {
        model.SetSkill(index, skillId);
        switch (index)
        {
            case 1:
                skill1.value = skillId;
                break;
            case 2:
                skill2.value = skillId;
                break;
            case 3:
                skill3.value = skillId;
                break;
            case 4:
                skill4.value = skillId;
                break;
            case 5:
                skillSpecial.value = skillId;
                break;
            default:
                break;
        }
    }

    public void CastSkill(int index)
    {
        var skill = this.model.GetSkill(index);
        if (IsCountDown(index))
        {
            MyPlayer.myPlayer.CastSkill(skill);
            //释放技能
        }
        else
        {
            DebugEx.Log("技能冷却中，无法释放。");
        }
    }

    public bool IsCountDown(int index)
    {
        var skill = this.model.GetSkill(index);
        var canCastTime = 0f;
        this.model.TryGetNextCastTime(skill, out canCastTime);
        return Time.realtimeSinceStartup < canCastTime;
    }

    public int GetSkillCountDown(int index)
    {
        var skill = this.model.GetSkill(index);

        var canCastTime = 0f;
        this.model.TryGetNextCastTime(skill, out canCastTime);
        var seconds = (int)(canCastTime - Time.realtimeSinceStartup);

        var countDown = seconds > 0 ? seconds : 0;
        return countDown;
    }

    public float GetSkillCountDownAmount(int index)
    {
        var skill = this.model.GetSkill(index);

        var canCastTime = 0f;
        this.model.TryGetNextCastTime(skill, out canCastTime);
        var seconds = canCastTime - Time.realtimeSinceStartup;
        var countDownTotal = 0f;
        this.model.TryGetTotalCountDown(skill, out countDownTotal);

        return Mathf.Clamp01(seconds / countDownTotal);
    }

    public SkillBaseInfo GetSkillBaseInfo(int index)
    {
        return new SkillBaseInfo()
        {
            name = string.Format("第{0}个技能", index),
            description = "这个技能很厉害",
        };
    }

    public struct SkillBaseInfo
    {
        public int icon;
        public string name;
        public string description;
    }


}





