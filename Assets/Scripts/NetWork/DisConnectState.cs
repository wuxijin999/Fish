using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisConnectState : NetState
{

    public override void Enter()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                DebugEx.Log("网络已断开！");
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                DebugEx.Log("网络正常，可以重连");
                break;
            default:
                break;
        }
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
    }

    public override bool CanExit()
    {
        return true;
    }
}
