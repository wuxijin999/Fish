using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmCancel : Presenter<ConfirmCancel>
{
    UnityAction confirmAction = null;
    UnityAction cancelAction = null;

    public string content;
    public string title;

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

    public ConfirmCancel Get()
    {
        confirmAction = null;
        cancelAction = null;

        content = string.Empty;
        title = string.Empty;

        return this;
    }

    public ConfirmCancel SetContent(string _content)
    {
        content = _content;
        return this;
    }

    public ConfirmCancel SetTitle(string _title)
    {
        title = _title;
        return this;
    }

    public ConfirmCancel OnConfirm(UnityAction _onConfirm)
    {
        confirmAction = _onConfirm;
        return this;
    }

    public ConfirmCancel OnCancel(UnityAction _onCancel)
    {
        cancelAction = _onCancel;
        return this;
    }

    public ConfirmCancel Begin()
    {
        Windows.Instance.Open(WindowType.ConfirmCancel);
        return this;
    }

    public void Confirm()
    {
        if (confirmAction != null)
        {
            confirmAction.Invoke();
        }
    }

    public void Cancel()
    {
        if (cancelAction != null)
        {
            cancelAction.Invoke();
        }
    }

}
