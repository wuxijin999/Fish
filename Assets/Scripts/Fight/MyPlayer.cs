using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IMyPlayer
{

}

public sealed class MyPlayer : FightActor, IMyPlayer
{

    public MyPlayer(int instanceId, ActorType actorType, Transform model)
        : base(instanceId, actorType, model)
    {
        if (model == null)
        {
            throw new ArgumentNullException("transform is null");
        }
    }

    public override void OnLateUpdate1()
    {
        base.OnLateUpdate1();
        if (JoyStick.direction != Vector2.zero)
        {
            var toPosition = position + new Vector3(JoyStick.direction.x, 0f, JoyStick.direction.y) * speed * Time.deltaTime;
            PushCommand(CommandType.Move, toPosition);
        }
    }

}

