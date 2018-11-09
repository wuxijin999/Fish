using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Hurt : LogicState_Base
{
    public LogicState_Hurt(ActionController controller) : base(controller)
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
