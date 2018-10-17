using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpNamePool
{

    static GameObjectPool pool = null;
    public static HeadUpName Get()
    {
        if (pool == null)
        {
            pool = GameObjectPoolUtil.Create(UIAssets.LoadPrefab("HeadUpName"));
        }

        var gameObject = pool.Get();
        return gameObject.GetComponent<HeadUpName>();
    }

    public static void Release(HeadUpName headUpName)
    {
        if (pool == null)
        {
            return;
        }

        pool.Release(headUpName.gameObject);
    }

}
