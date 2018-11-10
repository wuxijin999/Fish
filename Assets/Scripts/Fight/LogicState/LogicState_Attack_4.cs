﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Attack_4 : LogicState_Base
{

    public LogicState_Attack_4(Animator animator) : base(animator)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Attack4]);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

}