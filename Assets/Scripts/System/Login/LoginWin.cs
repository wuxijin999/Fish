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
        this.m_Login.SetListener(this.AccountLogin);
    }

    protected override void OnPreOpen()
    {
        Login.Instance.accountErrorEvent += this.OnAccountError;
        Login.Instance.passwordErrorEvent += this.OnPasswordError;
    }

    protected override void OnPreClose()
    {
        Login.Instance.accountErrorEvent -= this.OnAccountError;
        Login.Instance.passwordErrorEvent -= this.OnPasswordError;
    }

    #endregion

    private void AccountLogin()
    {
        Login.Instance.AccountLogin(this.m_Account.text, this.m_Password.text);
    }

    private void OnAccountError(int error)
    {

    }

    private void OnPasswordError(int error)
    {

    }


}





