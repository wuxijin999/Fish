//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindPreciousWin : Window
{

    [SerializeField] FunctionButton m_WorldBossButton;

    #region Built-in

    protected override void SetListeners()
    {
        m_WorldBossButton.SetListener(() => { FindPrecious.Instance.OpenWindow(1); });
    }

    protected override void OnPreOpen()
    {
    }

    protected override void OnPreClose()
    {
    }

    protected override void OnActived()
    {
        base.OnActived();

        SwitchWidgets();
    }
    #endregion


    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (FindPrecious.Instance.functionId.dirty)
        {
            SwitchWidgets();
        }
    }

    private void SwitchWidgets()
    {
        var functionId = FindPrecious.Instance.functionId.Fetch();
        SetWidgetActive<WorldBossWidget>(functionId == 1);
    }

}





