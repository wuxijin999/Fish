using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseWait : Presenter<PleaseWait>
{

    List<WaitType> waitings = new List<WaitType>();

    public override void Init()
    {
    }

    public override void UnInit()
    {
    }

    public override void OnSwitchAccount()
    {
    }

    public override void OnLoginOk()
    {
    }

    public void Show(WaitType waitType, float _delay = 0f)
    {
        if (!waitings.Contains(waitType))
        {
            waitings.Add(waitType);
        }

        if (!Windows.Instance.IsOpen(WindowType.PleaseWait))
        {
            Windows.Instance.Open(WindowType.PleaseWait);
        }
    }

    public void Hide(WaitType waitType)
    {
        if (waitings.Contains(waitType))
        {
            waitings.Remove(waitType);
        }

        if (waitings.Count == 0 && Windows.Instance.IsOpen(WindowType.PleaseWait))
        {
            Windows.Instance.Close(WindowType.PleaseWait);
        }
    }

    public enum WaitType
    {
        NetLink = 1,
        WindowLoad = 2,
    }

}

