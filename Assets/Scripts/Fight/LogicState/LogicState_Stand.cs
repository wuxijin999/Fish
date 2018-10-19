using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class LogicState_Stand : LogicState_Base
    {

        public override void Enter()
        {
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
}

