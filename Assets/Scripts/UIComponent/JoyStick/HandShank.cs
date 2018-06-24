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

    public Vector3 center {
        get {
            if (m_BackGround != null)
            {
                return m_BackGround.transform.position.SetZ(this.transform.position.z);
            }
            else
            {
                return this.transform.position;
            }
        }
    }

    protected void Awake()
    {
        Hide(m_HideOnRelease);
        ResetPosition();
    }

    private void OnEnable()
    {
        Hide(m_HideOnRelease);
        ResetPosition();

        state = HandShankState.UnActive;
    }

    private void OnDisable()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_Camera = eventData.pressEventCamera ?? m_Camera;

        m_Fore.gameObject.SetActive(true);
        m_BackGround.gameObject.SetActive(true);
        m_Arrow.gameObject.SetActive(true);

        var mousePosition = AmendMousePosition(eventData.position);
        m_Fore.transform.position = mousePosition;

        if (m_ClampRectTransform != null)
        {
            var min = UIUtility.GetMinWorldPosition(m_ClampRectTransform);
            var max = UIUtility.GetMaxWorldPosition(m_ClampRectTransform);
            mousePosition = mousePosition.SetX(Mathf.Clamp(mousePosition.x, min[0], max[0]));
            mousePosition = mousePosition.SetY(Mathf.Clamp(mousePosition.y, min[1], max[1]));
            m_BackGround.position = mousePosition;
        }
        else
        {
            m_BackGround.position = mousePosition;
        }

        UpdateArrowDirection();

        state = HandShankState.Active;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_Camera = eventData.pressEventCamera ?? m_Camera;
        UpdatePosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Hide(m_HideOnRelease);
        ResetPosition();
        state = HandShankState.UnActive;
    }

    private void UpdatePosition(Vector3 _pressPosition)
    {
        var mousePosition = AmendMousePosition(_pressPosition);
        var distance = Vector2.Distance(center, mousePosition);
        if (distance < 0.001f)
        {
            return;
        }
        else
        {
            if (m_Radius > distance)
            {
                m_Fore.transform.position = mousePosition;
            }
            else
            {
                var t = m_Radius / distance;
                m_Fore.transform.position = Vector3.Lerp(center, mousePosition, t);
            }
        }

        UpdateArrowDirection();
    }

    private void Update()
    {
        if (state == HandShankState.Active)
        {
            var direction = CalculateDirection();
            switch (m_Mode)
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
        if (state == HandShankState.UnActive)
        {
            return Vector2.zero;
        }
        else
        {
            var direction = Vector2.zero;
            direction = Vector3.Normalize(m_Fore.transform.position - center);
            return direction;
        }
    }

    private float CalculateSpeedRate()
    {
        if (state == HandShankState.UnActive)
        {
            return 0f;
        }
        else
        {
            return Mathf.Clamp(Vector3.Distance(m_Fore.transform.position, center) / m_Radius, 0, 1f);
        }
    }

    private Vector3 AmendMousePosition(Vector2 _pressPosition)
    {
        var position = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, _pressPosition, m_Camera, out position);
        return position;
    }

    private void UpdateArrowDirection()
    {
        if (m_Arrow != null)
        {
            var relativePosition = (m_Fore.position - m_BackGround.position).normalized;
            var targetAngle = relativePosition.x > 0f ? 360f - Vector3.Angle(relativePosition, Vector3.up) : Vector3.Angle(relativePosition, Vector3.up);
            m_Arrow.transform.localEulerAngles = new Vector3(0, 0, targetAngle);
        }
    }

    private void Hide(bool _hide)
    {
        m_Fore.gameObject.SetActive(!_hide);
        m_BackGround.gameObject.SetActive(!_hide);
        m_Arrow.gameObject.SetActive(false);
    }

    private void ResetPosition()
    {
        m_BackGround.anchoredPosition = m_ClampRectTransform.anchoredPosition;
        m_Fore.anchoredPosition = m_ClampRectTransform.anchoredPosition;
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



