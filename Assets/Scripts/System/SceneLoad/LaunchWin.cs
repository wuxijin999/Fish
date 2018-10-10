//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchWin : Window
{
    [SerializeField] ImageEx m_BackGround;
    [SerializeField] SmoothSlider m_Slider;
    [SerializeField] TextEx m_Tips;

    #region Built-in

    protected override void SetListeners()
    {
    }

    protected override void OnPreOpen()
    {
        DisplayBackGround();
        DisplayProgress();
        DisplayTips();
    }

    protected override void OnPreClose()
    {
    }

    public override void OnLateUpdate()
    {
        if (LaunchPresenter.Instance.progress.dirty)
        {
            DisplayProgress();
        }

        if (LaunchPresenter.Instance.randowTips.dirty)
        {
            DisplayTips();
        }
    }

    #endregion

    private void DisplayBackGround()
    {
    }

    private void DisplayProgress()
    {
        m_Slider.value = LaunchPresenter.Instance.progress.Fetch();
    }

    private void DisplayTips()
    {
        m_Tips.SetText(Language.GetLocal(LaunchPresenter.Instance.randowTips.Fetch()));
    }

}





