//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWin : Window
{
    [SerializeField] ImageEx m_BackGround;
    [SerializeField] SmoothSlider m_Slider;

    #region Built-in

    protected override void SetListeners()
    {

    }

    protected override void OnPreOpen()
    {
        DisplayBackGround();
        DisplayProgress();
    }

    protected override void OnPreClose()
    {
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (LoadingUI.Instance.progress.dirty)
        {
            DisplayProgress();
        }
    }

    #endregion

    private void DisplayBackGround()
    {
        var sceneId = LoadingUI.Instance.sceneId.Fetch();
        var config = MapConfig.Get(sceneId);
        m_BackGround.SetSprite(config.backGround);
    }

    private void DisplayProgress()
    {
        m_Slider.value = LoadingUI.Instance.progress.Fetch();
    }


}





