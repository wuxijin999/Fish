using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameObjectPoolUtil
{
    public static readonly Vector3 HIDE_POINT = new Vector3(0, 5000, 0);

    static int instanceId = 1000;
    static Dictionary<int, GameObjectPool> pools = new Dictionary<int, GameObjectPool>();

    public static GameObjectPool Create(GameObject prefab)
    {
        instanceId++;
        var pool = new GameObjectPool(instanceId, prefab);
        pools.Add(instanceId, pool);
        return pool;
    }

    public static bool Destroy(GameObjectPool pool)
    {
        if (pool == null)
        {
            return false;
        }

        if (pools.ContainsKey(pool.instanceId))
        {
            pools.Remove(pool.instanceId);
        }

        pool.Destroy();

        return true;
    }

#if UNITY_EDITOR
    public static void CreatePoolDebugWindow()
    {
        EditorHelper.gameObjectPools = pools;
    }
#endif
}
