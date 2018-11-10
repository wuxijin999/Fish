using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Jump : LogicState_Base
{
    public LogicState_Jump(Animator animator) : base(animator)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Jump]);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
