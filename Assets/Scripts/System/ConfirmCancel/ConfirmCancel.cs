using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmCancel : Presenter<ConfirmCancel>
{
    UnityAction confirmAction = null;
    UnityAction cancelAction = null;

    public string content { get; private set; }
    public string title { get; private set; }

    public override void OpenWindow()
    {
    }

    public override void CloseWindow()
    {
    }

    public ConfirmCancel Get()
    {
        this.confirmAction = null;
        this.cancelAction = null;

        this.content = string.Empty;
        this.title = string.Empty;

        return this;
    }

    public ConfirmCancel SetContent(string _content)
    {
        this.content = _content;
        return this;
    }

    public ConfirmCancel SetTitle(string _title)
    {
        this.title = _title;
        return this;
    }

    public ConfirmCancel OnConfirm(UnityAction _onConfirm)
    {
        this.confirmAction = _onConfirm;
        return this;
    }

    public ConfirmCancel OnCancel(UnityAction _onCancel)
    {
        this.cancelAction = _onCancel;
        return this;
    }

    public ConfirmCancel Begin()
    {
        Windows.Instance.Open(WindowType.ConfirmCancel);
        return this;
    }

    public void Confirm()
    {
        if (this.confirmAction != null)
        {
            this.confirmAction.Invoke();
        }
    }

    public void Cancel()
    {
        if (this.cancelAction != null)
        {
            this.cancelAction.Invoke();
        }
    }


}
