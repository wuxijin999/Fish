using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IOtherPlayer
{

}

public sealed class OtherPlayer : FightActor, IOtherPlayer
{
    public OtherPlayer(int instanceId, ActorType type, Transform transform)
        : base(instanceId, type, transform)
    {
        if (transform == null)
        {
            throw new ArgumentNullException("model is null");
        }

    }

}

