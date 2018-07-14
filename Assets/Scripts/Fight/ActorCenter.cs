using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCenter : Singleton<ActorCenter>
{

    int instanceId = 10000;

    public ActorBase Get()
    {
        instanceId++;
        var actor = new ActorBase();
        actor.instanceId = instanceId;

        return actor;
    }

}
