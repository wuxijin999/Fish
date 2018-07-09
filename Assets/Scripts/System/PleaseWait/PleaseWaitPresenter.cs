using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseWaitPresenter : Presenter<PleaseWaitPresenter>
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

    public void Show(WaitType _waitType, float _delay = 0f)
    {
        if (!waitings.Contains(_waitType))
        {
            waitings.Add(_waitType);
        }

        if (!WindowCenter.Instance.CheckOpen<PleaseWaitWin>())
        {
            WindowCenter.Instance.Open<PleaseWaitWin>(true);
        }
    }

    public void Hide(WaitType _waitType)
    {
        if (waitings.Contains(_waitType))
        {
            waitings.Remove(_waitType);
        }

        if (waitings.Count == 0 && WindowCenter.Instance.CheckOpen<PleaseWaitWin>())
        {
            WindowCenter.Instance.Close<PleaseWaitWin>(true);
        }
    }



    public enum WaitType
    {
        NetLink = 1,
        WindowLoad = 2,
    }

}

