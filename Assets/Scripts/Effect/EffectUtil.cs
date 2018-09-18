using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUtil : Singleton<EffectUtil>
{

    Dictionary<int, GameObjectPool> effectPools = new Dictionary<int, GameObjectPool>();

    public EffectBehaviour Play(int id, Transform parent = null)
    {
        try
        {
            if (!effectPools.ContainsKey(id))
            {
                var prefab = EffectAssets.LoadEffect(id);
                effectPools[id] = GameObjectPoolUtil.Create(prefab);
            }

            var pool = effectPools[id];
            var instance = pool.Get();
            var behaviour = instance.GetComponent<EffectBehaviour>();
            behaviour.effectId = id;

            if (!behaviour.gameObject.activeInHierarchy)
            {
                behaviour.gameObject.SetActive(true);
            }

            behaviour.OnPlay(parent);
            return behaviour;
        }
        catch (System.Exception ex)
        {
            DebugEx.Log(ex);
            return null;
        }
    }

    public void Stop(EffectBehaviour effect)
    {
        try
        {
            if (effect == null)
            {
                return;
            }
            var id = effect.effectId;
            var pool = effectPools[id];
            effect.OnStop();

            if (pool != null)
            {
                pool.Release(effect.gameObject);
            }
        }
        catch (System.Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

}
