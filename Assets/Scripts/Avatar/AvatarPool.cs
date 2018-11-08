using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPool
{
    static Dictionary<int, GameObjectPool> pools = new Dictionary<int, GameObjectPool>();

    public static GameObject GetBody(int id)
    {
        if (!pools.ContainsKey(id))
        {
            var prefab = MobAssets.LoadPrefab(id);
            pools[id] = GameObjectPoolUtil.Create(prefab);
        }

        var pool = pools[id];
        return pool.Get();
    }

    public static void Release(int id, GameObject gameObject)
    {
        if (pools.ContainsKey(id))
        {
            pools[id].Release(gameObject);
        }
    }

}
