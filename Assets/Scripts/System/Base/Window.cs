using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] protected RectTransform m_BackGround;
    [SerializeField] protected RectTransform m_Content;

    int m_Function = 0;
    public int function
    {
        get { return m_Function; }
        private set { m_Function = value; }
    }

    public void Open(int _function)
    {

    }

    public void Close(bool _immediately)
    {

    }

}
