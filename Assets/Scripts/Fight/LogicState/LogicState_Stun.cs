using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class LogicState_Stun : LogicState_Base
    {

        public override void Enter()
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
}

