using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorEngine : SingletonMonobehaviour<ActorEngine>
{

    public event System.Action onFixedUpdateEvent;

    public event System.Action onUpdateEvent1;
    public event System.Action onUpdateEvent2;

    public event System.Action onLateUpdateEvent1;
    public event System.Action onLateUpdateEvent2;

    public void Launch()
    {

    }

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
