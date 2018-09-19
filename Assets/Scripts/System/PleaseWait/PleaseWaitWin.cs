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
        this.backGround = this.transform.Find("Image");
        this.circle = this.transform.Find("Img_Circle");
    }

    protected override void SetListeners()
    {
    }

    protected override void OnPreOpen()
    {
        this.timer = 0f;
        if (linkOverTime > 0.001f)
        {
            this.actived = false;
            this.backGround.gameObject.SetActive(false);
            this.circle.gameObject.SetActive(false);
        }
        else
        {
            this.actived = true;
            this.backGround.gameObject.SetActive(true);
            this.circle.gameObject.SetActive(true);
        }
    }

    #endregion

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        this.timer += Time.deltaTime;
        if (!this.actived && this.timer > linkOverTime)
        {
            this.backGround.gameObject.SetActive(true);
            this.circle.gameObject.SetActive(true);
        }
    }

}

