﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmCancelWin : Window
{

    [SerializeField] TextMeshProUGUI m_Title;
    [SerializeField] TextMeshProUGUI m_Description;

    [SerializeField] ButtonEx m_Cancel;
    [SerializeField] ButtonEx m_Confirm;

    protected override void BindController()
    {
    }

    protected override void SetListeners()
    {
        this.m_Confirm.AddListener(this.OnConfirm);
        this.m_Cancel.AddListener(this.OnCancel);
    }

    protected override void OnPreOpen()
    {
        if (!string.IsNullOrEmpty(ConfirmCancel.Instance.title))
        {
            this.m_Title.text = ConfirmCancel.Instance.title;
        }

        if (!string.IsNullOrEmpty(ConfirmCancel.Instance.content))
        {
            this.m_Description.text = ConfirmCancel.Instance.content;
        }
    }

    private void OnConfirm()
    {
        ConfirmCancel.Instance.Confirm();
        this.Close();
    }

    private void OnCancel()
    {
        ConfirmCancel.Instance.Cancel();
        this.Close();
    }
}
