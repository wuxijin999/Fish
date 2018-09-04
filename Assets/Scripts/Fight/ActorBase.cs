using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBase
{

    int m_InstanceId = 0;
    public int instanceId
    {
        get { return m_InstanceId; }
        set { m_InstanceId = value; }
    }

    bool m_Enable = false;
    public bool enable
    {
        get { return m_Enable; }
        set
        {
            ActorEngine.Instance.onFixedUpdateEvent -= OnFixedUpdate;
            ActorEngine.Instance.onUpdateEvent1 -= OnUpdate1;
            ActorEngine.Instance.onUpdateEvent2 -= OnUpdate2;
            ActorEngine.Instance.onLateUpdateEvent1 -= OnLateUpdate1;
            ActorEngine.Instance.onLateUpdateEvent2 -= OnLateUpdate2;

            m_Enable = value;

            if (m_Enable)
            {
                ActorEngine.Instance.onFixedUpdateEvent += OnFixedUpdate;
                ActorEngine.Instance.onUpdateEvent1 += OnUpdate1;
                ActorEngine.Instance.onUpdateEvent2 += OnUpdate2;
                ActorEngine.Instance.onLateUpdateEvent1 += OnLateUpdate1;
                ActorEngine.Instance.onLateUpdateEvent2 += OnLateUpdate2;
            }
        }
    }

    ActorBrainState m_BrainState = ActorBrainState.Sane;
    public ActorBrainState brainState
    {
        get { return m_BrainState; }
        set { m_BrainState = value; }
    }

    public float speed { get; set; }

    ActionController m_ActionController = null;
    public ActionController actionController { get { return m_ActionController; } }

    public readonly PathFinder pathFinder;
    public readonly Transform transform;

    public ActorBase(Transform transform)
    {
        this.transform = transform;
        pathFinder = new PathFinder(this);
    }

    protected virtual void OnFixedUpdate()
    {
    }

    protected virtual void OnUpdate1()
    {
    }

    protected virtual void OnUpdate2()
    {
    }

    protected virtual void OnLateUpdate1()
    {
    }

    protected virtual void OnLateUpdate2()
    {
    }

    private void OnGetValue(System.IntPtr stmt)
    {
    }
}


public enum ActorBrainState
{
    Sane = 1,                   //理智状态，可以正常思考
    Obstinate = 2,            //执着于做某件事情，直到做完为止
    Lost = 3,                    //失去控制，完全不能控制自己的行为
}


