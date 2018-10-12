using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterWorldState : NetState
{
    const float OVER_TIME = 15f;
    bool isOverTime = false;
    Clock clock;

    public override void Enter()
    {
        PleaseWait.Instance.Show(PleaseWait.WaitType.NetLink, 0f);

        isOverTime = false;

        var param = new Clock.ClockParams()
        {
            type = Clock.ClockType.UnityRealTimeClock,
            second = OVER_TIME,
        };

        clock = ClockUtil.Instance.Create(param, () => { isOverTime = true; });
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
        if (clock != null)
        {
            clock.Stop();
        }

        if (isOverTime)
        {
            DebugEx.LogFormat("进入游戏世界超时！");
        }

        PleaseWait.Instance.Hide(PleaseWait.WaitType.NetLink);
    }

    public override bool CanExit()
    {
        return isOverTime || !Login.Instance.enterWorlding.value;
    }
}
