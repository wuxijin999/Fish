using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameObjectPoolUtility
{
    static int instanceId = 1000;
    static Dictionary<int, UIGameObjectPool> pools = new Dictionary<int, UIGameObjectPool>();

    public static UIGameObjectPool Create(GameObject _prefab)
    {
        instanceId++;
        var pool = new UIGameObjectPool(instanceId, _prefab);
        pools.Add(instanceId, pool);
        return pool;
    }

    public static bool Destroy(UIGameObjectPool _pool)
    {
        if (_pool == null)
        {
            return false;
        }

        if (pools.ContainsKey(_pool.instanceId))
        {
            pools.Remove(_pool.instanceId);
        }

        _pool.Destroy();

        return true;
    }

}
