using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmCancelPresenter : Presenter<ConfirmCancelPresenter>
{
    public UnityAction confirmAction { get; private set; }
    public UnityAction cancelAction { get; private set; }

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

    public ConfirmCancelPresenter SimpleConfirmCancel()
    {
        confirmAction = null;
        cancelAction = null;

        content = string.Empty;
        title = string.Empty;

        return this;
    }

    public ConfirmCancelPresenter SetContent(string _content)
    {
        content = _content;
        return this;
    }

    public ConfirmCancelPresenter SetTitle(string _title)
    {
        title = _title;
        return this;
    }

    public ConfirmCancelPresenter OnConfirm(UnityAction _onConfirm)
    {
        confirmAction = _onConfirm;
        return this;
    }

    public ConfirmCancelPresenter OnCancel(UnityAction _onCancel)
    {
        cancelAction = _onCancel;
        return this;
    }

    public ConfirmCancelPresenter Begin()
    {
        WindowCenter.Instance.Open<ConfirmCancelWin>();
        return this;
    }


}
