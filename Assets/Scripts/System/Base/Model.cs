using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model<T> where T : class, new()
{
    static T m_Instance;
    public static T Instance
    {
        get { return m_Instance ?? (m_Instance = new T()); }
    }

    protected Model()
    {

    }

    public abstract void Init();
    public abstract void UnInit();
    public abstract void OnSwitchAccount();
    public abstract void OnLoginOk();

}
