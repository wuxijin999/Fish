//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillModel : Model
{
    Dictionary<int, int> indexToSkills = new Dictionary<int, int>();
    Dictionary<int, float> skillNextCastTimes = new Dictionary<int, float>();
    Dictionary<int, float> skillCountDown = new Dictionary<int, float>();

    public override void Reset()
    {
    }

    public void SetSkill(int index, int skillId)
    {
        indexToSkills[index] = skillId;
    }

    public int GetSkill(int index)
    {
        var skill = 0;
        this.indexToSkills.TryGetValue(index, out skill);
        return skill;
    }

    public void SetNextCastTime(int skillId, float time)
    {
        skillNextCastTimes[skillId] = time;
        skillCountDown[skillId] = time - Time.realtimeSinceStartup;
    }

    public bool TryGetNextCastTime(int skillId, out float time)
    {
        return this.skillNextCastTimes.TryGetValue(skillId, out time);
    }

    public bool TryGetTotalCountDown(int skillId, out float countDown)
    {
        return this.skillCountDown.TryGetValue(skillId, out countDown);
    }

}





