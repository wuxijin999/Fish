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

    public Tween Play(bool _forward = true)
    {
        m_OnComplete.RemoveAllListeners();

        switch (m_Type)
        {
            case TweenType.Alpha:
                alphaFrom = _forward ? m_AlphaFrom : m_AlphaTo;
                alphaTo = _forward ? m_AlphaTo : m_AlphaFrom;
                break;
            case TweenType.Position:
            case TweenType.Rotation:
            case TweenType.Scale:
                from = _forward ? m_From : m_To;
                to = _forward ? m_To : m_From;
                break;
        }

        Begin(m_Type);

        return this;
    }

    public Tween Play(TweenType _type, Vector3 _from, Vector3 _to, float _duration)
    {
        m_OnComplete.RemoveAllListeners();

        switch (_type)
        {
            case TweenType.Position:
            case TweenType.Rotation:
            case TweenType.Scale:
                from = _from;
                to = _to;
                m_Duration = _duration;
                Begin(_type);
                break;
            default:
                return this;
        }

        return this;
    }

    public Tween Play(TweenType _type, float _from, float _to, float _duration)
    {
        m_OnComplete.RemoveAllListeners();

        switch (_type)
        {
            case TweenType.Alpha:
                alphaFrom = Mathf.Clamp01(_from);
                alphaTo = Mathf.Clamp01(_to);
                m_Duration = _duration;
                Begin(_type);
                return this;
            default:
                return this;
        }
    }

    public Tween OnComplete(UnityAction _onComplete)
    {
        m_OnComplete.RemoveAllListeners();
        m_OnComplete.AddListener(_onComplete);

        return this;
    }

    void Begin(TweenType _type)
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

        switch (_type)
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

    private void BeginPosition(float _delay, float _duration, int _loopTimes, LoopType _loopType)
    {
        if (m_IsLocal)
        {
            if (m_IsUI)
            {
                var rectTransform = this.transform as RectTransform;
                rectTransform.anchoredPosition = from;
                rectTransform.DOLocalMove(to, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
            }
            else
            {
                this.transform.localPosition = from;
                this.transform.DOLocalMove(to, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
            }
        }
        else
        {
            this.transform.position = from;
            this.transform.DOMove(to, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
        }
    }

    private void BeginRotation(float _delay, float _duration, int _loopTimes, LoopType _loopType)
    {
        if (m_IsLocal)
        {
            this.transform.localEulerAngles = from;
            this.transform.DOLocalRotate(to, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
        }
        else
        {
            this.transform.eulerAngles = from;
            this.transform.DORotate(to, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
        }
    }

    private void BeginScale(float _delay, float _duration, int _loopTimes, LoopType _loopType)
    {
        this.transform.localScale = from;
        this.transform.DOScale(to, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
    }

    private void BeginFade(float _delay, float _duration, int _loopTimes, LoopType _loopType)
    {
        canvasGroup.alpha = alphaFrom;
        canvasGroup.DOFade(alphaTo, _duration).SetDelay(_delay).SetEase(m_Ease).OnComplete(OnComplete).SetLoops(_loopTimes, _loopType);
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
