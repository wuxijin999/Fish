using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectState : NetState
{
    Clock detectClock;
    bool isDisconnected = false;

    public override void Enter()
    {
        isDisconnected = false;
        Fish.AddLisenter(BroadcastType.ApplicationUnPause, OnApplicationUnPause);
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
        if (detectClock != null)
        {
            detectClock.Stop();
        }
        Fish.RemoveLisenter(BroadcastType.ApplicationUnPause, OnApplicationUnPause);
    }

    public override bool CanExit()
    {
        return isDisconnected;
    }

    void OnApplicationUnPause()
    {
        if (detectClock != null)
        {
            detectClock.Stop();
        }

        var param = new Clock.ClockParams()
        {
            type = Clock.ClockType.UnityRealTimeClock,
            second = 3,
        };

        detectClock = ClockUtil.Instance.Create(param, () => { isDisconnected = GameNet.Instance.connected; });
    }

}
