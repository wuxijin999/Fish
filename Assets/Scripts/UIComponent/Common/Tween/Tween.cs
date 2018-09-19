using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Tween : MonoBehaviour
{
    [SerializeField] TweenType m_Type = TweenType.Position;
    public TweenType type { get { return this.m_Type; } }

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
    public float duration { get { return this.m_Duration; } }

    [SerializeField] Ease m_Ease = Ease.Linear;
    [SerializeField] UIEvent m_OnComplete = null;

    CanvasGroup m_CanvasGroup;
    CanvasGroup canvasGroup { get { return this.m_CanvasGroup ?? (this.m_CanvasGroup = this.AddMissingComponent<CanvasGroup>()); } }

    Vector3 from;
    Vector3 to;

    float alphaFrom = 1f;
    float alphaTo = 1f;

    private void OnEnable()
    {
        if (this.m_Trigger == Trigger.Enable)
        {
            switch (this.m_Type)
            {
                case TweenType.Alpha:
                    this.alphaFrom = this.m_AlphaFrom;
                    this.alphaTo = this.m_AlphaTo;
                    break;
                case TweenType.Position:
                case TweenType.Rotation:
                case TweenType.Scale:
                    this.from = this.m_From;
                    this.to = this.m_To;
                    break;
            }

            Begin(this.m_Type);
        }
    }

    private void Start()
    {
        if (this.m_Trigger == Trigger.Start)
        {
            switch (this.m_Type)
            {
                case TweenType.Alpha:
                    this.alphaFrom = this.m_AlphaFrom;
                    this.alphaTo = this.m_AlphaTo;
                    break;
                case TweenType.Position:
                case TweenType.Rotation:
                case TweenType.Scale:
                    this.from = this.m_From;
                    this.to = this.m_To;
                    break;
            }

            Begin(this.m_Type);
        }
    }

    private void OnDisable()
    {
        this.transform.DOKill(false);
    }

    public Tween Play(bool forward = true)
    {
        this.m_OnComplete.RemoveAllListeners();

        switch (this.m_Type)
        {
            case TweenType.Alpha:
                this.alphaFrom = forward ? this.m_AlphaFrom : this.m_AlphaTo;
                this.alphaTo = forward ? this.m_AlphaTo : this.m_AlphaFrom;
                break;
            case TweenType.Position:
            case TweenType.Rotation:
            case TweenType.Scale:
                this.from = forward ? this.m_From : this.m_To;
                this.to = forward ? this.m_To : this.m_From;
                break;
        }

        Begin(this.m_Type);

        return this;
    }

    public Tween Play(TweenType type, Vector3 from, Vector3 to, float duration)
    {
        this.m_OnComplete.RemoveAllListeners();

        switch (type)
        {
            case TweenType.Position:
            case TweenType.Rotation:
            case TweenType.Scale:
                this.from = from;
                this.to = to;
                this.m_Duration = duration;
                Begin(type);
                break;
            default:
                return this;
        }

        return this;
    }

    public Tween Play(TweenType type, float from, float to, float duration)
    {
        this.m_OnComplete.RemoveAllListeners();

        switch (type)
        {
            case TweenType.Alpha:
                this.alphaFrom = Mathf.Clamp01(from);
                this.alphaTo = Mathf.Clamp01(to);
                this.m_Duration = duration;
                Begin(type);
                return this;
            default:
                return this;
        }
    }

    public Tween OnComplete(UnityAction onComplete)
    {
        this.m_OnComplete.RemoveAllListeners();
        this.m_OnComplete.AddListener(onComplete);

        return this;
    }

    void Begin(TweenType type)
    {
        var delay = this.m_Delay;
        if (delay < 0f)
        {
            delay = 0f;
        }

        var duration = this.m_Duration;
        if (duration < 0f)
        {
            duration = 0f;
        }

        var loopTimes = 0;
        var loopType = LoopType.Restart;

        switch (this.m_WrapMode)
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
        if (this.m_IsLocal)
        {
            if (this.m_IsUI)
            {
                var rectTransform = this.transform as RectTransform;
                rectTransform.anchoredPosition = this.from;
                rectTransform.DOLocalMove(this.to, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
            }
            else
            {
                this.transform.localPosition = this.from;
                this.transform.DOLocalMove(this.to, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
            }
        }
        else
        {
            this.transform.position = this.from;
            this.transform.DOMove(this.to, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
        }
    }

    private void BeginRotation(float delay, float duration, int loopTimes, LoopType loopType)
    {
        if (this.m_IsLocal)
        {
            this.transform.localEulerAngles = this.from;
            this.transform.DOLocalRotate(this.to, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
        }
        else
        {
            this.transform.eulerAngles = this.from;
            this.transform.DORotate(this.to, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
        }
    }

    private void BeginScale(float delay, float duration, int loopTimes, LoopType loopType)
    {
        this.transform.localScale = this.from;
        this.transform.DOScale(this.to, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
    }

    private void BeginFade(float delay, float duration, int loopTimes, LoopType loopType)
    {
        this.canvasGroup.alpha = this.alphaFrom;
        this.canvasGroup.DOFade(this.alphaTo, duration).SetDelay(delay).SetEase(this.m_Ease).OnComplete(this.OnComplete).SetLoops(loopTimes, loopType);
    }

    private void OnComplete()
    {
        if (this.m_OnComplete != null)
        {
            this.m_OnComplete.Invoke();
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
