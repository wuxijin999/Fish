using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Dance : LogicState_Base
{
    public LogicState_Dance(ActionController controller) : base(controller)
    {
    }

    public override bool CanTransit(LogicStateType stateType)
    {
        return true;
    }

    public override void Enter(object value)
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
