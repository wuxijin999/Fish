//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, November 08, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttackButton : UIBase, IPointerDownHandler, IPointerUpHandler
{
    const float PRESS_TRIGGERTIME = 0.2f;
    readonly UIEvent attackEvent = new UIEvent();

    bool m_IsDown = false;
    public bool isDown {
        get {
            if (Application.isMobilePlatform)
            {
                return Input.touchCount > 0 && m_IsDown;
            }
            else
            {
                return Input.GetMouseButton(0) && m_IsDown;
            }
        }
    }

    bool clickResponse = false;
    float pressTime = 0f;
    bool m_IsPress = false;

    public void SetListener(UnityAction _action)
    {
        attackEvent.AddListener(_action);
    }

    public void RemoveListener()
    {
        attackEvent.RemoveAllListeners();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsDown = true;
        m_IsPress = false;
        clickResponse = true;
        pressTime = Time.time + PRESS_TRIGGERTIME;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_IsDown = false;
        m_IsPress = false;
        clickResponse = false;
    }

    protected void OnDisable()
    {
        m_IsDown = false;
        m_IsPress = false;
        clickResponse = false;
    }

    private void LateUpdate()
    {
        if (!isDown)
        {
            return;
        }

        if (Time.time > pressTime)
        {
            m_IsPress = true;
        }

        if (!m_IsPress && clickResponse)
        {
            attackEvent.Invoke();
            clickResponse = false;
        }

        if (m_IsPress)
        {
            attackEvent.Invoke();
        }
    }

}




