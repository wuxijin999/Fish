using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumPool
{
    static Dictionary<int, GameObjectPool> pools = new Dictionary<int, GameObjectPool>();

    public static DamageNum Get(DamageNum.Pattern pattern)
    {
        var intPattern = (int)pattern;
        if (pools.ContainsKey(intPattern))
        {
            pools[intPattern] = GameObjectPoolUtil.Create(UIAssets.LoadPrefab("DamageNum_" + intPattern));
        }

        var pool = pools[intPattern];
        var gameObject = pool.Get();
        return gameObject.GetComponent<DamageNum>();
    }

    public static void Release(DamageNum damageNum)
    {
        if (damageNum == null)
        {
            Debug.Log("被回收对象是空的，不要忽悠我。");
            return;
        }

        var intPattern = (int)damageNum.pattern;
        if (pools.ContainsKey(intPattern))
        {
            pools[intPattern].Release(damageNum.gameObject);
        }
    }

}
