﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillCast : Presenter<SkillCast>
{

    SkillModel skillModel = new SkillModel();

    public override void OpenWindow()
    {
    }

    public override void CloseWindow()
    {
    }

    public void CastSkill(int index)
    {
        var skill = this.skillModel.GetSkill(index);

        if (IsCountDown(index))
        {
            //释放技能
        }
        else
        {
            DebugEx.Log("技能冷却中，无法释放。");
        }
    }

    public bool IsCountDown(int index)
    {
        var skill = this.skillModel.GetSkill(index);
        DateTime canCastTime;
        this.skillModel.TryGetNextCastTime(skill, out canCastTime);
        return DateTime.Now < canCastTime;
    }

    public int GetSkillCountDown(int index)
    {
        var skill = this.skillModel.GetSkill(index);

        DateTime canCastTime;
        this.skillModel.TryGetNextCastTime(skill, out canCastTime);
        var seconds = (int)(canCastTime - DateTime.Now).TotalSeconds;

        var countDown = seconds > 0 ? seconds : 0;
        return countDown;
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




