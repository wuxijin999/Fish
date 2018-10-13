//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, July 09, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsWin : Window
{
    [SerializeField] PopupTipsWidget m_NormalTip;

    #region Built-in
    protected override void SetListeners()
    {
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
        DisplayNormalTip();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (Tips.Instance.hasNewNormalTip.dirty)
        {
            DisplayNormalTip();
        }
    }

    #endregion

    private void DisplayNormalTip()
    {
        if (Tips.Instance.hasNewNormalTip.Fetch())
        {
            m_NormalTip.SetActive(true);
            while (Tips.Instance.tipsQueue[Tips.Type.Normal].Count > 0)
            {
                var tip = Tips.Instance.tipsQueue[Tips.Type.Normal].Dequeue();
                m_NormalTip.Popup(tip);
            }
        }
    }

}





