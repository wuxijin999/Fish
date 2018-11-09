using System.Collections;
using System.Collections.Generic;

public abstract class LogicState_Base
{
    protected ActionController controller;

    public LogicState_Base(ActionController controller)
    {
        this.controller = controller;
    }

    public abstract void Enter(object value);
    public abstract void Update();
    public abstract void Exit();
    public abstract bool CanTransit(LogicStateType stateType);
}

