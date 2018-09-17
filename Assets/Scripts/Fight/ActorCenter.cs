using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCenter : Singleton<ActorCenter>
{

    int instanceId = 10000;
    Dictionary<int, ActorBase> actors = new Dictionary<int, ActorBase>();

    public ActorBase Get(Transform model)
    {
        instanceId++;
        var actor = new ActorBase(model);
        actor.instanceId = instanceId;

        return actor;
    }


    public ActorBase FindNearestEmeny(float radius, Vector3 center)
    {
        var centerXZ = new Vector2(center.x, center.z);
        var distance = 99999f;
        ActorBase emeny = null;
        foreach (var item in actors.Values)
        {
            if (item.alive && item.actorType == ActorType.Emeny)
            {
                var xz = new Vector2(item.transform.position.x, item.transform.position.z);
                var tempDistance = Vector2.Distance(centerXZ, xz);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    emeny = item;
                }
            }
        }

        return emeny;
    }

    public ActorBase FindNearestEmeny(float radius, Vector3 center, Vector2 left, Vector2 right)
    {
        var centerXZ = new Vector2(center.x, center.z);
        var distance = 99999f;
        ActorBase emeny = null;
        foreach (var item in actors.Values)
        {
            if (item.alive && item.actorType == ActorType.Emeny)
            {
                var xz = new Vector2(item.transform.position.x, item.transform.position.z);
                var tempDistance = Vector2.Distance(centerXZ, xz);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    emeny = item;
                }
            }
        }

        return emeny;
    }

}
