using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PleaseWaitWin : Window
{
    static float linkOverTime = 0f;

    Transform backGround;
    Transform circle;

    float timer = 0f;
    bool actived = false;

    #region Built-in
    protected override void BindController()
    {
        backGround = this.transform.Find("Image");
        circle = this.transform.Find("Img_Circle");
    }

    protected override void SetListeners()
    {
    }

    protected override void OnPreOpen()
    {
        timer = 0f;
        if (linkOverTime > 0.001f)
        {
            actived = false;
            backGround.gameObject.SetActive(false);
            circle.gameObject.SetActive(false);
        }
        else
        {
            actived = true;
            backGround.gameObject.SetActive(true);
            circle.gameObject.SetActive(true);
        }
    }

    protected override void OnAfterOpen()
    {
    }

    protected override void OnPreClose()
    {
    }

    protected override void OnAfterClose()
    {
    }
    #endregion

    protected override void LateUpdate()
    {
        base.LateUpdate();
        timer += Time.deltaTime;

        if (!actived && timer > linkOverTime)
        {
            backGround.gameObject.SetActive(true);
            circle.gameObject.SetActive(true);
        }

    }
}

