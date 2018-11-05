//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, September 12, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : Presenter<Login>, IPresenterWindow
{

    LoginModel model = new LoginModel();

    string localSaveAccount {
        get { return LocalSave.GetString("LocalSave_Account"); }
        set { LocalSave.SetString("LocalSave_Account", value); }
    }

    string localSavePassword {
        get { return LocalSave.GetString("LocalSave_Password"); }
        set { LocalSave.SetString("LocalSave_Password", value); }
    }

    string accountBuf;
    string passwordBuf;

    public readonly IntProperty accountError = new IntProperty();
    public readonly IntProperty passwordError = new IntProperty();

    public readonly BoolProperty accountLogining = new BoolProperty();
    public readonly BoolProperty enterWorlding = new BoolProperty();

    public void OpenWindow(object @object = null)
    {
        Windows.Instance.Open(WindowType.Login);
    }

    public void CloseWindow()
    {
        Windows.Instance.Close(WindowType.Login);
    }

    public void AccountLogin(string account, string password)
    {
        var error = 0;
        if (!IsValidAccount(account, out error))
        {
            accountError.value = error;
            return;
        }

        if (!IsValidPassword(password, out error))
        {
            this.passwordError.value = error;
            return;
        }

        accountBuf = account;
        passwordBuf = password;
        model.Reset();
        accountLogining.value = true;
    }

    public void ReAccountLogin()
    {
        AccountLogin(localSaveAccount, localSavePassword);
    }

    public bool IsAccountLoginOk()
    {
        return model.accountLoginOk;
    }

    public bool IsEnterWorldOk()
    {
        return model.enterWorldOk;
    }

    private void OnAccountLoginResult(bool ok, string result)
    {
        if (ok)
        {
            localSaveAccount = accountBuf;
            localSavePassword = passwordBuf;
        }
    }

    private bool IsValidAccount(string account, out int error)
    {
        if (string.IsNullOrEmpty(account))
        {
            error = 1;
            return false;
        }

        if (account.Length > 6)
        {
            error = 2;
            return false;
        }

        error = 0;
        return true;
    }

    private bool IsValidPassword(string password, out int error)
    {
        if (string.IsNullOrEmpty(password))
        {
            error = 1;
            return false;
        }

        if (password.Length < 6)
        {
            error = 2;
            return false;
        }

        error = 0;
        return true;
    }


}





