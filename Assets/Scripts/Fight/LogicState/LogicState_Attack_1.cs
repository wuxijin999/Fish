using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Attack_1 : LogicState_Base
{

    public LogicState_Attack_1(Animator animator) : base(animator)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Attack1]);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

}
