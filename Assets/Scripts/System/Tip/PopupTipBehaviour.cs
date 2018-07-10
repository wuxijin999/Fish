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
public class PopupTipBehaviour : UIBehaviour
{
    [SerializeField] TextMeshProUGUI m_Content;
    [SerializeField] Tween m_Tween;
    [SerializeField] CanvasGroup m_CanvasGroup;

    public float fadeOutTime { get; set; }

    RectTransform m_RecTransform;
    public RectTransform rectTransform { get { return m_RecTransform ?? (this.transform as RectTransform); } }

    public void Popup(string _content, float _fromY, float _toY, float _duration)
    {
        m_CanvasGroup.alpha = 1f;
        m_Content.text = _content;
        Move(_fromY, _toY, _duration);
    }

    public void Move(float _fromY, float _toY, float _duration)
    {
        var from = rectTransform.anchoredPosition.SetY(_fromY);
        var to = rectTransform.anchoredPosition.SetY(_toY);
        m_Tween.Play(Tween.TweenType.Position, from, to, _duration);
    }

    public void FadeOut(float _fromAlpha, float _toAlpha, float _duration, UnityAction<PopupTipBehaviour> _callBack)
    {
        m_Tween.Play(Tween.TweenType.Alpha, _fromAlpha, _toAlpha, _duration).OnComplete(() =>
        {
            if (_callBack != null)
            {
                _callBack(this);
            }
        });
    }


}



