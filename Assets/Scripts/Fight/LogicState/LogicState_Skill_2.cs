using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Skill_2 : LogicState_Base
{
    int skillId = 0;
    public LogicState_Skill_2(Animator animator) : base(animator)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
        skillId = (int)value;
        this.animator.SetTrigger(LogicController.stateHashs[ActionType.Skill2]);
    }

    public override void Exit()
    {
        skillId = 0;
    }

    public override void Update()
    {
    }
}
