//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, July 09, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class PopupTipBehaviour : UIBase
{
    [SerializeField] TextEx m_Content;
    [SerializeField] Tween m_Tween;
    [SerializeField] CanvasGroup m_CanvasGroup;

    public float fadeOutTime { get; set; }

    public void Popup(string content, float fromY, float toY, float duration)
    {
        this.m_CanvasGroup.alpha = 1f;
        this.m_Content.text = content;
        Move(fromY, toY, duration);
    }

    public void Move(float fromY, float toY, float duration)
    {
        var from = this.rectTransform.anchoredPosition.SetY(fromY);
        var to = this.rectTransform.anchoredPosition.SetY(toY);
        this.m_Tween.Play(Tween.TweenType.Position, from, to, duration);
    }

    public void FadeOut(float fromAlpha, float toAlpha, float duration, UnityAction<PopupTipBehaviour> callBack)
    {
        this.m_Tween.Play(Tween.TweenType.Alpha, fromAlpha, toAlpha, duration).OnComplete(() =>
        {
            if (callBack != null)
            {
                callBack(this);
            }
        });
    }


}



