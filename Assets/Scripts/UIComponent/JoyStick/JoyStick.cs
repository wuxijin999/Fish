//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Wednesday, January 24, 2018
//--------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static Vector2 direction;

    [SerializeField] RectTransform m_Fore;
    [SerializeField] RectTransform m_BackGround;
    [SerializeField] RectTransform m_MovePort;
    [SerializeField] RectTransform m_Arrow;

    [SerializeField] bool m_HideOnRelease = true;
    [SerializeField] float m_Radius = 1f;

    RectTransform rectTransform { get { return this.transform as RectTransform; } }

    Vector3 center {
        get {
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

    HandShankState state = HandShankState.UnActive;

    private void OnEnable()
    {
        Hide(this.m_HideOnRelease);
        ResetPosition();
        this.state = HandShankState.UnActive;
    }

    private void OnDisable()
    {
        direction = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_Fore.SetActive(true);
        this.m_BackGround.SetActive(true);
        if (this.m_Arrow)
        {
            this.m_Arrow.SetActive(true);
        }

        UpdateArrowDirection();

        var mousePosition = ClampMousePosition(eventData.position, eventData.pressEventCamera);
        this.m_Fore.transform.position = mousePosition;

        var backGroundPosition = mousePosition;
        if (this.m_MovePort != null)
        {
            var min = UIUtil.GetMinWorldPosition(this.m_MovePort);
            var max = UIUtil.GetMaxWorldPosition(this.m_MovePort);
            backGroundPosition = backGroundPosition.SetX(Mathf.Clamp(mousePosition.x, min[0], max[0]));
            backGroundPosition = backGroundPosition.SetY(Mathf.Clamp(mousePosition.y, min[1], max[1]));
        }
        this.m_BackGround.position = backGroundPosition;

        this.state = HandShankState.Active;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdatePosition(eventData.position, eventData.pressEventCamera);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Hide(this.m_HideOnRelease);
        ResetPosition();
        this.state = HandShankState.UnActive;
    }

    private void UpdatePosition(Vector3 pressPosition, Camera camera)
    {
        var mousePosition = ClampMousePosition(pressPosition, camera);
        var distance = Vector2.Distance(this.center, mousePosition);
        if (distance > 0.001f)
        {
            var t = Mathf.Clamp01(this.m_Radius * 0.01f / distance);
            this.m_Fore.transform.position = Vector3.Lerp(this.center, mousePosition, t);
        }

        UpdateArrowDirection();
    }

    private void Update()
    {
        if (this.state == HandShankState.Active)
        {
            direction = CalculateDirection();
        }
        else
        {
            direction = Vector2.zero;
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
            var direction = this.m_Fore.transform.position - this.center;
            var distance = Mathf.Clamp01(direction.magnitude / (m_Radius * 0.01f));

            return direction.normalized * distance;
        }
    }

    private Vector3 ClampMousePosition(Vector2 _pressPosition, Camera camera)
    {
        var position = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.rectTransform, _pressPosition, camera, out position);
        return position;
    }

    private void UpdateArrowDirection()
    {
        if (this.m_Arrow != null)
        {
            var relativePosition = (this.m_Fore.position - this.m_BackGround.position).normalized;
            var acuteAngle = Vector3.Angle(relativePosition, Vector3.up);
            var angle = relativePosition.x > 0f ? 360f - acuteAngle : acuteAngle;
            this.m_Arrow.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void Hide(bool hide)
    {
        this.m_Fore.SetActive(!hide);
        this.m_BackGround.SetActive(!hide);
        if (this.m_Arrow)
        {
            this.m_Arrow.SetActive(false);
        }
    }

    private void ResetPosition()
    {
        this.m_BackGround.anchoredPosition = this.m_MovePort.anchoredPosition;
        this.m_Fore.anchoredPosition = this.m_MovePort.anchoredPosition;
    }

    public enum HandShankState
    {
        Active,
        UnActive,
    }

}



