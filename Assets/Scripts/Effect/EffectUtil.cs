using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUtil : Singleton<EffectUtil>
{

    Dictionary<int, GameObjectPool> effectPools = new Dictionary<int, GameObjectPool>();
    Dictionary<int, EffectBehaviour> playingEffect = new Dictionary<int, EffectBehaviour>();

    int playInstanceId = 1;

    public int Play(int id, Transform parent = null)
    {
        try
        {
            if (!this.effectPools.ContainsKey(id))
            {
                var prefab = EffectAssets.LoadEffect(id);
                this.effectPools[id] = GameObjectPoolUtil.Create(prefab);
            }

            var pool = this.effectPools[id];
            var instance = pool.Get();
            var behaviour = instance.GetComponent<EffectBehaviour>();
            behaviour.effectId = id;

            if (!behaviour.gameObject.activeInHierarchy)
            {
                behaviour.SetActive(true);
            }

            playInstanceId++;
            playingEffect[playInstanceId] = behaviour;
            behaviour.OnPlay(playInstanceId, parent);
            return playInstanceId;
        }
        catch (System.Exception ex)
        {
            DebugEx.Log(ex);
            return 0;
        }
    }

    public void Stop(int instanceId)
    {
        if (!playingEffect.ContainsKey(instanceId))
        {
            return;
        }

        var effect = playingEffect[instanceId];
        playingEffect.Remove(instanceId);
        if (effect == null)
        {
            return;
        }

        try
        {
            effect.OnStop();

            var id = effect.effectId;
            var pool = this.effectPools[id];
            if (pool != null)
            {
                pool.Release(effect.gameObject);
            }
            else
            {
                effect.gameObject.DestroySelf();
            }
        }
        catch (System.Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

}
