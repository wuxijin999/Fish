using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerverConnectState : NetState
{

    public override void Enter()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
    }

    public override bool CanExit()
    {
        return Login.Instance.accountLogining.value;
    }

}
