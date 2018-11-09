using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCenter : Singleton<ActorCenter>
{

    int instanceId = 10000;
    Dictionary<int, ActorBase> actors = new Dictionary<int, ActorBase>();

    public ActorBase Create(ActorType actorType, Transform model)
    {
        if (model == null)
        {
            Debug.LogWarning("ActorBase 创建时依赖的 Transform 为 null ,这是禁止的！");
            return null;
        }
        else
        {
            this.instanceId++;
            switch (actorType)
            {
                case ActorType.Player:
                    return new MyPlayer(++this.instanceId, actorType, model);
                case ActorType.Emeny:
                    return new FightActor(++this.instanceId, actorType, model);
                case ActorType.NPC:
                    return new ActorBase(++this.instanceId, actorType, model);
                default:
                    return new ActorBase(++this.instanceId, actorType, model);
            }

        }
    }

    public bool TryGet(int instanceId, out ActorBase actorBase)
    {
        return this.actors.TryGetValue(instanceId, out actorBase);
    }

    public List<int> GetActors(ActorType type)
    {
        var temp = new List<int>();
        foreach (var item in this.actors.Values)
        {
            if (item != null && item.actorType == type)
            {
                temp.Add(item.instanceId);
            }
        }

        return temp;
    }

    public int FindNearestEmeny(Vector3 center, float radius)
    {
        var centerXZ = new Vector2(center.x, center.z);
        var distance = 99999f;
        ActorBase emeny = null;
        foreach (var item in this.actors.Values)
        {
            if (item == null)
            {
                continue;
            }

            if (item.alive && item.actorType == ActorType.Emeny)
            {
                var xz = new Vector2(item.transform.position.x, item.transform.position.z);
                var tempDistance = Vector2.Distance(centerXZ, xz);
                if (tempDistance < radius && tempDistance < distance)
                {
                    distance = tempDistance;
                    emeny = item;
                }
            }
        }

        return emeny.instanceId;
    }

    public int FindNearestEmeny(Vector3 center, Vector3 forward, float radius, float angleRange)
    {
        var centerXZ = new Vector2(center.x, center.z);
        var forwardXZ = new Vector2(forward.x, forward.z);
        var distance = 99999f;
        ActorBase emeny = null;
        var directionXZ = forwardXZ - centerXZ;

        foreach (var item in this.actors.Values)
        {
            if (item == null)
            {
                continue;
            }

            if (item.alive && item.actorType == ActorType.Emeny)
            {
                var xz = new Vector2(item.transform.position.x, item.transform.position.z);
                var tempDistance = Vector2.Distance(centerXZ, xz);
                if (tempDistance < radius && tempDistance < distance)
                {
                    var angle = Vector2.Angle((xz - centerXZ), directionXZ);
                    if (Mathf.Abs(angle) < angleRange * 0.5f)
                    {
                        distance = tempDistance;
                        emeny = item;
                    }
                }
            }
        }

        return emeny.instanceId;
    }

}

