﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Skill_1 : LogicState_Base
{

    int skillId = 0;
    public LogicState_Skill_1(Animator animator) : base(animator)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
        skillId = (int)value;
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Skill1]);
    }

    public override void Exit()
    {
        skillId = 0;
    }

    public override void Update()
    {
    }
}
