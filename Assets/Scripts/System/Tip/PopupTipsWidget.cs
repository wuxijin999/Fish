//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, July 09, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopupTipsWidget : Widget
{

    [SerializeField] float m_PopupSpeed = 10f;
    public float popupSpeed { get { return Mathf.Clamp(m_PopupSpeed, 5f, 1000f); } }

    [SerializeField] RectTransform m_StartPoint;
    [SerializeField] RectTransform m_PriorPoint;

    UIGameObjectPool m_Pool;
    UIGameObjectPool pool { get { return m_Pool ?? (UIGameObjectPoolUtility.Create(UILoader.LoadPrefab("PopupTipBehaviour"))); } }

    Queue<PopupTipBehaviour> activedBehaviours = new Queue<PopupTipBehaviour>();

    public override void AddListeners()
    {
    }

    public override void BindControllers()
    {
    }

    public void Popup(string _tip)
    {
        var instance = pool.Get();
        var behaviour = instance.GetComponent<PopupTipBehaviour>();

        var from = m_StartPoint.anchoredPosition.y;
        var to = m_PriorPoint.anchoredPosition.y;
        var duration = (from - to) / popupSpeed;

        behaviour.Popup(_tip, from, to, duration);
    }


}



