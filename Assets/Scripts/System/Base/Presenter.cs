//---------------------------------------------------
//
//
//---------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public abstract class Presenter<T> where T : class, new()
{

    static T m_Instance;
    public static T Instance {
        get { return m_Instance ?? (m_Instance = new T()); }
    }

    protected Presenter()
    {

    }

    public abstract void OpenWindow(int functionId = 0);
    public abstract void CloseWindow();

}
