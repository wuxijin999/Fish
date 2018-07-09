using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmCancelWin : Window
{

    [SerializeField] TextMeshProUGUI m_Title;
    [SerializeField] TextMeshProUGUI m_Content;

    [SerializeField] ButtonEx m_Cancel;
    [SerializeField] ButtonEx m_Confirm;

    private void OnConfirm()
    {
        if (ConfirmCancalPresenter.Instance.confirmAction != null)
        {
            ConfirmCancalPresenter.Instance.confirmAction.Invoke();
        }
    }

    private void OnCancel()
    {
        if (ConfirmCancalPresenter.Instance.cancelAction != null)
        {
            ConfirmCancalPresenter.Instance.cancelAction.Invoke();
        }
    }

}
