//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Wednesday, January 24, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class HandShank : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public event Action<float, Vector2> DirectionUpdateEvent;

    public Camera m_Camera { get; set; }

    [SerializeField] RectTransform m_Fore;
    [SerializeField] RectTransform m_ClampRectTransform;
    [SerializeField] RectTransform m_BackGround;
    [SerializeField] RectTransform m_Arrow;
    [SerializeField] Mode m_Mode;
    [SerializeField] bool m_HideOnRelease;
    [SerializeField] float m_Radius = 1f;

    public RectTransform rectTransform { get { return this.transform as RectTransform; } }

    const string HorizontalAxisName = "Horizontal";
    const string VerticalAxisName = "Vertical";

    HandShankState state;

    public Vector3 center
    {
        get
        {
            if (this.m_BackGround != null)
            {
                return this.m_BackGround.transform.position.SetZ(this.transform.position.z);
            }
            else
            {
                return this.transform.position;
            }
        }
    }

    protected void Awake()
    {
        Hide(this.m_HideOnRelease);
        ResetPosition();
    }

    private void OnEnable()
    {
        Hide(this.m_HideOnRelease);
        ResetPosition();

        this.state = HandShankState.UnActive;
    }

    private void OnDisable()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_Camera = eventData.pressEventCamera ?? this.m_Camera;

        this.m_Fore.gameObject.SetActive(true);
        this.m_BackGround.gameObject.SetActive(true);
        this.m_Arrow.gameObject.SetActive(true);

        var mousePosition = AmendMousePosition(eventData.position);
        this.m_Fore.transform.position = mousePosition;

        if (this.m_ClampRectTransform != null)
        {
            var min = UIUtil.GetMinWorldPosition(this.m_ClampRectTransform);
            var max = UIUtil.GetMaxWorldPosition(this.m_ClampRectTransform);
            mousePosition = mousePosition.SetX(Mathf.Clamp(mousePosition.x, min[0], max[0]));
            mousePosition = mousePosition.SetY(Mathf.Clamp(mousePosition.y, min[1], max[1]));
            this.m_BackGround.position = mousePosition;
        }
        else
        {
            this.m_BackGround.position = mousePosition;
        }

        UpdateArrowDirection();

        this.state = HandShankState.Active;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.m_Camera = eventData.pressEventCamera ?? this.m_Camera;
        UpdatePosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Hide(this.m_HideOnRelease);
        ResetPosition();
        this.state = HandShankState.UnActive;
    }

    private void UpdatePosition(Vector3 pressPosition)
    {
        var mousePosition = AmendMousePosition(pressPosition);
        var distance = Vector2.Distance(this.center, mousePosition);
        if (distance < 0.001f)
        {
            return;
        }
        else
        {
            if (this.m_Radius > distance)
            {
                this.m_Fore.transform.position = mousePosition;
            }
            else
            {
                var t = this.m_Radius / distance;
                this.m_Fore.transform.position = Vector3.Lerp(this.center, mousePosition, t);
            }
        }

        UpdateArrowDirection();
    }

    private void Update()
    {
        if (this.state == HandShankState.Active)
        {
            var direction = CalculateDirection();
            switch (this.m_Mode)
            {
                case Mode.OnlyDirection:
                    break;
                case Mode.SpeedAndDirection:
                    var speed = CalculateSpeedRate();
                    break;
            }

        }
    }

    private Vector2 CalculateDirection()
    {
        if (this.state == HandShankState.UnActive)
        {
            return Vector2.zero;
        }
        else
        {
            var direction = Vector2.zero;
            direction = Vector3.Normalize(this.m_Fore.transform.position - this.center);
            return direction;
        }
    }

    private float CalculateSpeedRate()
    {
        if (this.state == HandShankState.UnActive)
        {
            return 0f;
        }
        else
        {
            return Mathf.Clamp(Vector3.Distance(this.m_Fore.transform.position, this.center) / this.m_Radius, 0, 1f);
        }
    }

    private Vector3 AmendMousePosition(Vector2 _pressPosition)
    {
        var position = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.rectTransform, _pressPosition, this.m_Camera, out position);
        return position;
    }

    private void UpdateArrowDirection()
    {
        if (this.m_Arrow != null)
        {
            var relativePosition = (this.m_Fore.position - this.m_BackGround.position).normalized;
            var targetAngle = relativePosition.x > 0f ? 360f - Vector3.Angle(relativePosition, Vector3.up) : Vector3.Angle(relativePosition, Vector3.up);
            this.m_Arrow.transform.localEulerAngles = new Vector3(0, 0, targetAngle);
        }
    }

    private void Hide(bool _hide)
    {
        this.m_Fore.gameObject.SetActive(!_hide);
        this.m_BackGround.gameObject.SetActive(!_hide);
        this.m_Arrow.gameObject.SetActive(false);
    }

    private void ResetPosition()
    {
        this.m_BackGround.anchoredPosition = this.m_ClampRectTransform.anchoredPosition;
        this.m_Fore.anchoredPosition = this.m_ClampRectTransform.anchoredPosition;
    }

    public enum HandShankState
    {
        Active,
        UnActive,
    }

    public enum Mode
    {
        OnlyDirection,
        SpeedAndDirection,
    }

}



