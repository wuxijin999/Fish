using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Stun : LogicState_Base
{
    public LogicState_Stun(Animator animator) : base(animator)
    {
    }

    public override void Enter(object value)
    {
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Stun]);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        switch (stateType)
        {
            case LogicStateType.Dead:
                return true;
            default:
                return false;
        }
    }

}

