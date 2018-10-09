//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillModel
{

    Dictionary<int, int> indexToSkills = new Dictionary<int, int>();
    Dictionary<int, float> skillNextCastTimes = new Dictionary<int, float>();

    public int GetSkill(int index)
    {
        var skill = 0;
        this.indexToSkills.TryGetValue(index, out skill);
        return skill;
    }

    public bool TryGetNextCastTime(int skillId, out float time)
    {
        return this.skillNextCastTimes.TryGetValue(skillId, out time);
    }


}





