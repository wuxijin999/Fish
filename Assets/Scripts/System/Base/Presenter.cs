public interface IPresenterInit
{
    void Init();
}

public interface IPresenterUnInit
{
    void UnInit();
}

public interface IPresenterOnSwitchAccount
{
    void OnSwitchAccount();
}

public interface IPresenterOnLoginOk
{
    void OnLoginOk();
}

public interface IPresenterReset
{
    void OnReset();
}

public interface IPresenterWindow
{
    void OpenWindow(object @object = null);
    void CloseWindow();
}

public abstract class Presenter<T> where T : class, new()
{
    static T m_Instance;
    public static T Instance {
        get { return m_Instance ?? (m_Instance = new T()); }
    }

    public readonly IntProperty functionId = new IntProperty();

    protected Presenter()
    {
        if (this is IPresenterInit)
        {
            var init = this as IPresenterInit;
            init.Init();
        }

        Fish.AddLisenter(BroadcastType.BeforeLogin, OnReset);
        Fish.AddLisenter(BroadcastType.LoginOk, OnLoginOk);
        Fish.AddLisenter(BroadcastType.SwitchAccount, OnSwitchAccount);
    }

    ~Presenter()
    {
        if (this is IPresenterUnInit)
        {
            var uninit = this as IPresenterUnInit;
            uninit.UnInit();
        }

        Fish.RemoveLisenter(BroadcastType.BeforeLogin, OnReset);
        Fish.RemoveLisenter(BroadcastType.LoginOk, OnLoginOk);
        Fish.RemoveLisenter(BroadcastType.SwitchAccount, OnSwitchAccount);
    }

    private void OnSwitchAccount()
    {
        if (this is IPresenterOnSwitchAccount)
        {
            var switchAccount = this as IPresenterOnSwitchAccount;
            switchAccount.OnSwitchAccount();
        }
    }

    private void OnLoginOk()
    {
        if (this is IPresenterOnLoginOk)
        {
            var loginOk = this as IPresenterOnLoginOk;
            loginOk.OnLoginOk();
        }
    }

    private void OnReset()
    {
        if (this is IPresenterReset)
        {
            var reset = this as IPresenterReset;
            reset.OnReset();
        }
    }

}
