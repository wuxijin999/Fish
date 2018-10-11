using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetState
{

    public abstract void Enter();
    public abstract void OnUpdate();
    public abstract void Exit();
    public abstract bool CanExit();

}
