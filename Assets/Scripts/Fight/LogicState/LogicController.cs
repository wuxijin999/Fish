using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{

    public class LogicController
    {
        Dictionary<LogicStateType, LogicState_Base> states = new Dictionary<LogicStateType, LogicState_Base>();

        public LogicStateType currentStateType { get; private set; }

        public readonly ActionController actionController;
        public bool stateCompleted { get { return actionController.stateCompleted; } }
        public int nextAction { get; set; }

        public LogicController( )
        {
            actionController = new ActionController();
            currentStateType = LogicStateType.Stand;
            EnterState(currentStateType);
        }

        public bool DoAction(LogicStateType stateType, string parameter = null)
        {
            var canDo = true;
            if (states.ContainsKey(this.currentStateType))
            {
                canDo = states[this.currentStateType].CanTransit(stateType);
            }

            nextAction = 0;
            if (canDo)
            {
                EnterState(stateType);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoActionQueue(LogicStateType stateType, string parameter = null)
        {
            nextAction = (int)stateType;
            return true;
        }

        public void Update()
        {
            this.states[this.currentStateType].Update();
            if (actionController.stateCompleted)
            {
                if (nextAction != 0)
                {
                    actionController.EnterState((ActionStateType)nextAction);
                }
                else
                {
                    var isFight = false;
                    actionController.EnterState(isFight ? ActionStateType.CombatIdle : ActionStateType.Idle);
                }
            }

            actionController.Update();
        }

        void EnterState(LogicStateType stateType)
        {
            if (states.ContainsKey(currentStateType))
            {
                states[currentStateType].Exit();
            }

            var state = states.ContainsKey(stateType) ? states[stateType] : states[stateType] = CreateLogicState(stateType);
            state.Enter();
        }

        private LogicState_Base CreateLogicState(LogicStateType state)
        {
            switch (state)
            {
                case LogicStateType.Stand:
                    return new LogicState_Stand();
                default:
                    return null;
            }
        }

    }

    public enum LogicStateType
    {
        Stand,
        Move,
        Attack,
        Skill,
        Silence,
        Stun,
        Twine,
        Dead,
        Up,                //浮空
        Diaup            //击飞
    }

}

