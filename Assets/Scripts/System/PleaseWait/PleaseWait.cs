using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseWait : Presenter<PleaseWait>
{

    List<WaitType> waitings = new List<WaitType>();

    public override void OpenWindow()
    {
        Windows.Instance.Open(WindowType.PleaseWait);
    }

    public override void CloseWindow()
    {
        Windows.Instance.Close(WindowType.PleaseWait);
    }

    public void Show(WaitType waitType, float _delay = 0f)
    {
        if (!waitings.Contains(waitType))
        {
            waitings.Add(waitType);
        }

        OpenWindow();
    }

    public void Hide(WaitType waitType)
    {
        if (waitings.Contains(waitType))
        {
            waitings.Remove(waitType);
        }

        if (waitings.Count == 0)
        {
            CloseWindow();
        }
    }



    public enum WaitType
    {
        NetLink = 1,
        WindowLoad = 2,
    }

}

