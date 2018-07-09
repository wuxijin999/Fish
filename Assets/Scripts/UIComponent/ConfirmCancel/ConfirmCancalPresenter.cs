using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmCancalPresenter : Presenter<ConfirmCancalPresenter>
{

    public UnityAction confirmAction { get; private set; }
    public UnityAction cancelAction { get; private set; }

    string content;
    string title;

    public ConfirmCancalPresenter Do(string _content)
    {
        confirmAction = null;
        cancelAction = null;

        content = string.Empty;
        title = string.Empty;

        return this;
    }

    public ConfirmCancalPresenter SetTitle(string _title)
    {
        title = _title;
        return this;
    }

    public ConfirmCancalPresenter OnConfirm(UnityAction _onConfirm)
    {
        confirmAction = _onConfirm;
        return this;
    }

    public ConfirmCancalPresenter OnCancel(UnityAction _onCancel)
    {
        cancelAction = _onCancel;
        return this;
    }

}
