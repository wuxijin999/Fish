using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectPlay : MonoBehaviour
{

    [SerializeField] int m_Id;

    EffectBehaviour effect;

    public void Play()
    {
        if (effect != null)
        {
            EffectUtil.Instance.Stop(effect);
        }

        effect = EffectUtil.Instance.Play(m_Id, this.transform);
    }

    public void Stop()
    {
        if (effect != null)
        {
            EffectUtil.Instance.Stop(effect);
        }

        effect = null;
    }

}
