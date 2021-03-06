﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Move : LogicState_Base
{

    public LogicState_Move(Animator animator) : base(animator)
    {
    }

    public override void Enter(object value)
    {
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Move]);
    }

    public override void Exit()
    {
    }

    public override bool CanTransit(LogicStateType state)
    {
        return true;
    }

    public override void Update()
    {
    }

}

