using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Idle : LogicState_Base
{
    public LogicState_Idle(ActionController controller) : base(controller)
    {
    }

    public override void Enter(object value)
    {
        this.controller.EnterState(ActionStateType.Idle);
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

