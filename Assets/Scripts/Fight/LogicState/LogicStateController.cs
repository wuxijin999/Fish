using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{

    public class LogicStateController
    {

        Dictionary<LogicState, LogicState_Base> states = new Dictionary<LogicState, LogicState_Base>();

        public FightActor owner { get; private set; }

        public LogicStateController(FightActor actor)
        {
            this.owner = actor;
        }

        public bool EnterState(LogicState logicState)
        {
            var canEnter = false;
            switch (logicState)
            {
                case LogicState.Stand:
                    canEnter = CanEnterStandState();
                    break;
                default:
                    break;
            }

            if (canEnter)
            {
                var stateInstance = states.ContainsKey(logicState) ? states[logicState] : states[logicState] = CreateLogicState(logicState);
                stateInstance.Enter();
            }

            return canEnter;
        }

        public void Update()
        {

        }

        private bool CanEnterStandState()
        {
            foreach (var item in states.Values)
            {
                if (item is LogicState_Stand)
                {
                    return false;
                }
            }

            return true;
        }

        private LogicState_Base CreateLogicState(LogicState state)
        {
            switch (state)
            {
                case LogicState.Stand:
                    return new LogicState_Stand(this.owner);
                default:
                    return null;
            }
        }

    }


    public enum LogicState
    {
        Stand,
        Walk,
        Run,
        Attack,
        Skill,
        Silence,
        Stun,
        Twine,

    }

}

