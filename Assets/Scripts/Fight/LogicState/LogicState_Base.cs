using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LogicState_Base
{
    protected Animator animator;

    public LogicState_Base(Animator animator)
    {
        this.animator = animator;
    }

    public abstract void Enter(object value);
    public abstract void Update();
    public abstract void Exit();
    public abstract bool CanTransit(LogicStateType stateType);
}

