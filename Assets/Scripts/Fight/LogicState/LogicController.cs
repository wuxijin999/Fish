using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicController
{
    Dictionary<LogicStateType, LogicState_Base> states = new Dictionary<LogicStateType, LogicState_Base>();

    ActorBase actor;
    public LogicStateType currentStateType { get; private set; }
    public readonly ActionController actionController;
    public bool stateCompleted { get { return actionController.stateCompleted; } }
    public int nextAction { get; set; }


    public LogicController(ActorBase actor)
    {
        this.actor = actor;
        var animator = this.actor.transform.GetComponent<Animator>();
        actionController = new ActionController();
        actionController.animator = animator;

        currentStateType = LogicStateType.Idle;
        EnterState(currentStateType, null);
    }

    public bool DoAction(LogicStateType stateType, object value = null)
    {
        var canDo = true;
        if (states.ContainsKey(this.currentStateType))
        {
            canDo = states[this.currentStateType].CanTransit(stateType);
        }

        nextAction = 0;
        if (canDo)
        {
            EnterState(stateType, value);
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
                actionController.EnterState(ActionStateType.Idle);
            }
        }

        actionController.Update();
    }

    void EnterState(LogicStateType stateType, object value)
    {
        if (states.ContainsKey(currentStateType))
        {
            states[currentStateType].Exit();
        }

        var state = states.ContainsKey(stateType) ? states[stateType] : states[stateType] = CreateLogicState(stateType);
        state.Enter(value);
    }

    private bool CanEnter(LogicStateType from, LogicStateType to)
    {
        if (to == LogicStateType.Idle)
        {
            return true;
        }
        else if ((int)from > 1000 && (int)to < 1000)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private LogicState_Base CreateLogicState(LogicStateType state)
    {
        switch (state)
        {
            case LogicStateType.Idle:
                return new LogicState_Idle(actionController);
            case LogicStateType.Move:
                return new LogicState_Move(actionController);
            default:
                return null;
        }
    }

}

public enum LogicStateType
{
    Idle = 1,
    Move = 2,
    Attack = 100,
    Skill = 200,
    Dead = 1001,
    Stun = 1002,
    Twine = 1003,
    Up = 1004,                //浮空
    Diaup = 1005            //击飞
}


