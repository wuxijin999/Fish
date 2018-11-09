using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_HurtDown : LogicState_Base
{

    public LogicState_HurtDown(ActionController controller) : base(controller)
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
