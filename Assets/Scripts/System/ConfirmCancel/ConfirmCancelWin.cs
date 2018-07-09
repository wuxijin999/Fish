using System.Collections;
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

    protected override void AddListeners()
    {
        m_Confirm.AddListener(OnConfirm);
        m_Cancel.AddListener(OnCancel);
    }

    protected override void OnPreOpen()
    {
        if (!string.IsNullOrEmpty(ConfirmCancelPresenter.Instance.title))
        {
            m_Title.text = ConfirmCancelPresenter.Instance.title;
        }

        if (!string.IsNullOrEmpty(ConfirmCancelPresenter.Instance.content))
        {
            m_Description.text = ConfirmCancelPresenter.Instance.content;
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

    private void OnConfirm()
    {
        if (ConfirmCancelPresenter.Instance.confirmAction != null)
        {
            ConfirmCancelPresenter.Instance.confirmAction.Invoke();
        }

        this.Close(true);
    }

    private void OnCancel()
    {
        if (ConfirmCancelPresenter.Instance.cancelAction != null)
        {
            ConfirmCancelPresenter.Instance.cancelAction.Invoke();
        }

        this.Close(true);
    }
}
