//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, November 05, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class GestureCatcher : UIBase, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    UIEvent m_OnClick = new UIEvent();

    [NonSerialized] public Vector2 deltaPosition = Vector2.zero;
    Vector2 prePosition = Vector2.zero;

    public void SetListener(UnityAction callBack)
    {
        m_OnClick.RemoveAllListeners();
        m_OnClick.AddListener(callBack);
    }

    public void RemoveListener()
    {
        m_OnClick.RemoveAllListeners();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_OnClick.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        deltaPosition = Vector2.zero;
        prePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        deltaPosition = eventData.position - prePosition;
        prePosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        deltaPosition = Vector2.zero;
        prePosition = eventData.position;
    }

}




