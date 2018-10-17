using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpGuildPool
{
    static GameObjectPool pool = null;
    public static HeadUpGuild Get()
    {
        if (pool == null)
        {
            pool = GameObjectPoolUtil.Create(UIAssets.LoadPrefab("HeadUpGuild"));
        }

        var gameObject = pool.Get();
        return gameObject.GetComponent<HeadUpGuild>();
    }

    public static void Release(HeadUpGuild headUpGuild)
    {
        if (pool == null)
        {
            return;
        }

        pool.Release(headUpGuild.gameObject);
    }
}
