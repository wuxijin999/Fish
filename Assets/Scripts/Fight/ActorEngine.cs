using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorEngine : Singleton<ActorEngine>
{

    public event Action onFixedUpdateEvent;

    public event Action onUpdateEvent1;
    public event Action onUpdateEvent2;

    public event Action onLateUpdateEvent1;
    public event Action onLateUpdateEvent2;

    private void FixedUpdate()
    {
        if (onFixedUpdateEvent != null)
        {
            onFixedUpdateEvent();
        }
    }

    private void Update()
    {
        if (onUpdateEvent1 != null)
        {
            onUpdateEvent1();
        }

        if (onUpdateEvent2 != null)
        {
            onUpdateEvent2();
        }
    }

    private void LateUpdate()
    {
        if (onLateUpdateEvent1 != null)
        {
            onLateUpdateEvent1();
        }

        if (onLateUpdateEvent2 != null)
        {
            onLateUpdateEvent2();
        }
    }


}
