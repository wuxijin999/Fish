//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, September 12, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : Presenter<Login>
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

    public BizEvent<int> accountErrorEvent = new BizEvent<int>();
    public BizEvent<int> passwordErrorEvent = new BizEvent<int>();

    public readonly BoolProperty accountLogining = new BoolProperty();
    public readonly BoolProperty enterWorlding = new BoolProperty();

    public override void OpenWindow(int functionId = 0)
    {
        Windows.Instance.Open(WindowType.Login);
    }

    public override void CloseWindow()
    {
        Windows.Instance.Close(WindowType.Login);
    }

    public void AccountLogin(string account, string password)
    {
        var accountError = 0;
        if (IsValidAccount(account, out accountError))
        {
            this.accountErrorEvent.Invoke(accountError);
            return;
        }

        var passwordError = 0;
        if (IsValidPassword(password, out passwordError))
        {
            this.passwordErrorEvent.Invoke(passwordError);
            return;
        }

        model.Reset();
        accountLogining.value = true;
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





