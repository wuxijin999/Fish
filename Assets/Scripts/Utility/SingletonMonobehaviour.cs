using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    static T m_Instance = null;
    public static T Instance {
        get {
            if (m_Instance == null)
            {
                var m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                DontDestroyOnLoad(m_Instance.gameObject);
            }

            return m_Instance;
        }
    }

    private void Awake()
    {
        m_Instance = this as T;
    }


}
