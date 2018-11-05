//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, September 12, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginWin : Window
{
    [SerializeField] ButtonEx m_Login;
    [SerializeField] TMP_InputField m_Account;

    #region Built-in

    protected override void SetListeners()
    {
        this.m_Login.SetListener(this.AccountLogin);
    }

    protected override void OnPreOpen()
    {
    }

    protected override void OnPreClose()
    {
    }

    protected override void OnActived()
    {
        base.OnActived();
        Login.Instance.accountError.Fetch();
        Login.Instance.passwordError.Fetch();
    }

    public void LateUpdate()
    {
        if (Login.Instance.accountError.dirty)
        {
            OnAccountError(Login.Instance.accountError.Fetch());
        }

        if (Login.Instance.passwordError.dirty)
        {
            OnPasswordError(Login.Instance.passwordError.Fetch());
        }
    }

    #endregion

    private void AccountLogin()
    {
        Login.Instance.AccountLogin(this.m_Account.text, "123456");
    }

    private void OnAccountError(int error)
    {

    }

    private void OnPasswordError(int error)
    {

    }


}





