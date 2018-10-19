using System.Collections;
using System.Collections.Generic;

namespace Actor
{
    public abstract class LogicState_Base
    {

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        public abstract bool CanTransit(LogicStateType stateType);
    }
}

