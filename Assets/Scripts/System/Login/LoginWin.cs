//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, September 12, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWin : Window
{

    [SerializeField] ButtonEx m_Login;
    [SerializeField] InputField m_Account;
    [SerializeField] InputField m_Password;

    #region Built-in

    protected override void SetListeners()
    {
        m_Login.SetListener(AccountLogin);
    }

    protected override void OnPreOpen()
    {
        Login.Instance.accountErrorEvent += OnAccountError;
        Login.Instance.passwordErrorEvent += OnPasswordError;
    }

    protected override void OnAfterOpen()
    {
    }

    protected override void OnPreClose()
    {
        Login.Instance.accountErrorEvent -= OnAccountError;
        Login.Instance.passwordErrorEvent -= OnPasswordError;
    }

    protected override void OnAfterClose()
    {
    }
    #endregion

    private void AccountLogin()
    {
        Login.Instance.AccountLogin(m_Account.text, m_Password.text);
    }

    private void OnAccountError(int error)
    {

    }

    private void OnPasswordError(int error)
    {

    }


}





