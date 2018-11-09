using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_Stun : LogicState_Base
{
    public LogicState_Stun(ActionController controller) : base(controller)
    {
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

