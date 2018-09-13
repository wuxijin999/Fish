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
    [SerializeField] TextMeshProUGUI m_Content;
    [SerializeField] Tween m_Tween;
    [SerializeField] CanvasGroup m_CanvasGroup;

    public float fadeOutTime { get; set; }

    public void Popup(string content, float fromY, float toY, float duration)
    {
        m_CanvasGroup.alpha = 1f;
        m_Content.text = content;
        Move(fromY, toY, duration);
    }

    public void Move(float fromY, float toY, float duration)
    {
        var from = rectTransform.anchoredPosition.SetY(fromY);
        var to = rectTransform.anchoredPosition.SetY(toY);
        m_Tween.Play(Tween.TweenType.Position, from, to, duration);
    }

    public void FadeOut(float fromAlpha, float toAlpha, float duration, UnityAction<PopupTipBehaviour> callBack)
    {
        m_Tween.Play(Tween.TweenType.Alpha, fromAlpha, toAlpha, duration).OnComplete(() =>
        {
            if (callBack != null)
            {
                callBack(this);
            }
        });
    }


}



