using System.Collections;
using System.Collections.Generic;

namespace Actor
{
    public abstract class LogicState_Base
    {

        public ActorBase owner { get; private set; }
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        public LogicState_Base(ActorBase actor)
        {
            this.owner = actor;
        }

    }
}

