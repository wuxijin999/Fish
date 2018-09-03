using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Tween : MonoBehaviour
{
    [SerializeField] TweenType m_Type = TweenType.Position;
    public TweenType type { get { return m_Type; } }

    [SerializeField] bool m_IsUI = true;
    [SerializeField] bool m_IsLocal = true;
    [SerializeField] Vector3 m_From = Vector3.zero;
    [SerializeField] Vector3 m_To = Vector3.zero;

    [Range(0, 1)] [SerializeField] float m_AlphaFrom = 1f;
    [Range(0, 1)] [SerializeField] float m_AlphaTo = 1f;

    [SerializeField] Trigger m_Trigger = Trigger.Enable;
    [SerializeField] WrapMode m_WrapMode = WrapMode.Once;
    [SerializeField] float m_Delay = 0f;
    [SerializeField] float m_Duration = 1f;
    public float duration { get { return m_Duration; } }

    [SerializeField] Ease m_Ease = Ease.Linear;
    [SerializeField] UIEvent m_OnComplete = null;

    CanvasGroup m_CanvasGroup;
    CanvasGroup canvasGroup { get { return m_CanvasGroup ?? (m_CanvasGroup = this.AddMissingComponent<CanvasGroup>()); } }

    Vector3 from;
    Vector3 to;

    float alphaFrom = 1f;
    float alphaTo = 1f;

    private void OnEnable()
    {
        if (m_Trigger == Trigger.Enable)
        {
            switch (m_Type)
            {
                case TweenType.Alpha:
                    alphaFrom = m_AlphaFrom;
                    alphaTo = m_AlphaTo;
                    break;
                case TweenType.Position:
                case TweenType.Rotation:
                case TweenType.Scale:
                    from = m_From;
                    to = m_To;
                    break;
            }

            Begin(m_Type);
        }
    }

    private void Start()
    {
        if (m_Trigger == Trigger.Start)
        {
            switch (m_Type)
            {
                case TweenType.Alpha:
                    alphaFrom = m_AlphaFrom;
                    alphaTo = m_AlphaTo;
                    break;
                case TweenType.Position:
                case TweenType.Rotation:
                case TweenType.Scale:
                    from = m_From;
                    to = m_To;
                    break;
            }

            Begin(m_Type);
        }
    }

    private void OnDisable()
    {
        this.transform.DOKill(false);
    }

    public Tween Play(bool forward = true)
    {
        m_OnComplete.RemoveAllListeners();

        switch (m_Type)
        {
            case TweenType.Alpha:
                alphaFrom = forward ? m_AlphaFrom : m_AlphaTo;
                alphaTo = forward ? m_AlphaTo : m_AlphaFrom;
                break;
            case TweenType.Position:
            case TweenType.Rotation:
            case TweenType.Scale:
                from = forward ? m_From : m_To;
                to = forward ? m_To : m_From;
                break;
        }

        Begin(m_Type);

        return this;
    }

    public Tween Play(TweenType type, Vector3 from, Vector3 to, float duration)
    {
        m_OnComplete.RemoveAllListeners();

        switch (type)
        {
            case TweenType.Position:
            case TweenType.Rotation:
            case TweenType.Scale:
                this.from = from;
                this.to = to;
                m_Duration = duration;
                Begin(type);
                break;
            default:
                return this;
        }

        return this;
    }

    public Tween Play(TweenType type, float from, float to, float duration)
    {
        m_OnComplete.RemoveAllListeners();

        switch (type)
        {
            case TweenType.Alpha:
                alphaFrom = Mathf.Clamp01(from);
                alphaTo = Mathf.Clamp01(to);
                m_Duration = duration;
                Begin(type);
                return this;
            default:
                return this;
        }
    }

    public Tween OnComplete(UnityAction onComplete)
    {
        m_OnComplete.RemoveAllListeners();
        m_OnComplete.AddListener(onComplete);

        return this;
    }

    void Begin(TweenType type)
    {
        var delay = m_Delay;
        if (delay < 0f)
        {
            delay = 0f;
        }

        var duration = m_Duration;
        if (duration < 0f)
        {
            duration = 0f;
        }

        var loopTimes = 0;
        var loopType = LoopType.Restart;

        switch (m_WrapMode)
        {
            case WrapMode.Once:
                loopTimes = 0;
                loopType = LoopType.Restart;
                break;
            case WrapMode.Loop:
                loopTimes = -1;
                loopType = LoopType.Restart;
                break;
            case WrapMode.PingPong:
                loopTimes = -1;
                loopType = LoopType.Yoyo;
                break;
        }

        switch (type)
        {
            case TweenType.Position:
                BeginPosition(delay, duration, loopTimes, loopType);
                break;
            case TweenType.Rotation:
                BeginRotation(delay, duration, loopTimes, loopType);
                break;
            case TweenType.Scale:
                BeginScale(delay, duration, loopTimes, loopType);
                break;
            case TweenType.Alpha:
                BeginFade(delay, duration, loopTimes, loopType);
                break;
        }
    }

    private void BeginPosition(float delay, float duration, int loopTimes, LoopType loopType)
    {
        if (m_IsLocal)
        {
            if (m_IsUI)
            {
                var rectTransform = this.transform as RectTransform;
                rectTransform.anchoredPosition = from;
                rectTransform.DOLocalMove(to, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
            }
            else
            {
                this.transform.localPosition = from;
                this.transform.DOLocalMove(to, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
            }
        }
        else
        {
            this.transform.position = from;
            this.transform.DOMove(to, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
        }
    }

    private void BeginRotation(float delay, float duration, int loopTimes, LoopType loopType)
    {
        if (m_IsLocal)
        {
            this.transform.localEulerAngles = from;
            this.transform.DOLocalRotate(to, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
        }
        else
        {
            this.transform.eulerAngles = from;
            this.transform.DORotate(to, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
        }
    }

    private void BeginScale(float delay, float duration, int loopTimes, LoopType loopType)
    {
        this.transform.localScale = from;
        this.transform.DOScale(to, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
    }

    private void BeginFade(float delay, float duration, int loopTimes, LoopType loopType)
    {
        canvasGroup.alpha = alphaFrom;
        canvasGroup.DOFade(alphaTo, duration).SetDelay(delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(loopTimes, loopType);
    }

    private void OnComplete()
    {
        if (m_OnComplete != null)
        {
            m_OnComplete.Invoke();
        }
    }

    public enum Trigger
    {
        Manual,
        Start,
        Enable,
    }

    public enum WrapMode
    {
        Once,
        Loop,
        PingPong,
    }

    public enum TweenType
    {
        Position,
        Rotation,
        Scale,
        Alpha,
    }

}
