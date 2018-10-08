//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Monday, August 14, 2017
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RedpointBehaviour : UIBase
{

    [SerializeField] int m_RedpointId;
    public int redpointId {
        get { return m_RedpointId; }
        set {
            m_RedpointId = value;
            redpoint = RedpointCenter.Instance.GetRedpoint(m_RedpointId);
            UpdateRedpoint(redpoint.state.Fetch(), redpoint.count.Fetch());
        }
    }

    [SerializeField] RectTransform m_SimpleRedpoint;
    public RectTransform simpleRedpoint { get { return m_SimpleRedpoint; } }

    [SerializeField] RectTransform m_QuantityRedpoint;
    public RectTransform quantityRedpoint { get { return m_QuantityRedpoint; } }

    [SerializeField] Text m_Quantity;
    public Text quantityText { get { return m_Quantity; } }

    [SerializeField] RectTransform m_FullRedpoint;

    Redpoint redpoint;

    protected virtual void OnEnable()
    {
        redpoint = RedpointCenter.Instance.GetRedpoint(redpointId);
        if (redpoint != null)
        {
            UpdateRedpoint(redpoint.state.Fetch(), redpoint.count.Fetch());
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (redpoint != null && (redpoint.state.dirty || redpoint.count.dirty))
        {
            UpdateRedpoint(redpoint.state.Fetch(), redpoint.count.Fetch());
        }
    }

    void UpdateRedpoint(RedPointState state, int count)
    {
        if (m_SimpleRedpoint != null)
        {
            m_SimpleRedpoint.gameObject.SetActive(state == RedPointState.Simple);
        }

        if (m_QuantityRedpoint != null)
        {
            m_QuantityRedpoint.gameObject.SetActive(state == RedPointState.Count);
        }

        if (m_Quantity != null)
        {
            m_Quantity.gameObject.SetActive(state == RedPointState.Count);
        }

        if (m_FullRedpoint != null)
        {
            m_FullRedpoint.gameObject.SetActive(state == RedPointState.Full);
        }

        if (state == RedPointState.Count && m_Quantity != null)
        {
            m_Quantity.text = count > 9 ? "N" : count >= 1 ? count.ToString() : "";
        }

    }

}




