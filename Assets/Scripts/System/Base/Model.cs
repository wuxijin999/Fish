using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model<T> where T : class, new()
{
    static T m_Instance;
    public static T Instance
    {
        get { return m_Instance ?? (m_Instance = new T()); }
    }

    protected Model()
    {

    }

    public virtual void Init()
    {

    }

    public virtual void UnInit()
    {

    }

    protected virtual void OnSwitchAccount()
    {

    }

    protected virtual void OnLoginOk()
    {

    }



}
