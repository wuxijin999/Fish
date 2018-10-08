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

    [SerializeField] RectTransform m_Simple;
    [SerializeField] RectTransform m_Count;
    [SerializeField] TextEx m_CountText;
    [SerializeField] RectTransform m_Full;

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
        if (m_Simple != null)
        {
            m_Simple.gameObject.SetActive(state == RedPointState.Simple);
        }

        if (m_Count != null)
        {
            m_Count.gameObject.SetActive(state == RedPointState.Count);
        }

        if (m_CountText != null)
        {
            m_CountText.gameObject.SetActive(state == RedPointState.Count);

            if (state == RedPointState.Count)
            {
                m_CountText.SetText(count > 9 ? "N" : count >= 1 ? count.ToString() : "");
            }
        }

        if (m_Full != null)
        {
            m_Full.gameObject.SetActive(state == RedPointState.Full);
        }

    }

}




