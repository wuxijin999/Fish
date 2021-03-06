﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Hurt : LogicState_Base
{
    public LogicState_Hurt(Animator animator) : base(animator)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Hurt]);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

}
