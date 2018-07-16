using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCenter : Singleton<ActorCenter>
{

    int instanceId = 10000;

    public ActorBase Get( Transform _model)
    {
        instanceId++;
        var actor = new ActorBase(_model);
        actor.instanceId = instanceId;

        return actor;
    }

}
