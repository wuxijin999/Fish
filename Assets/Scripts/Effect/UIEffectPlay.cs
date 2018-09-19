using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectPlay : MonoBehaviour
{

    [SerializeField] int m_Id;

    EffectBehaviour effect;

    public void Play()
    {
        if (this.effect != null)
        {
            EffectUtil.Instance.Stop(this.effect);
        }

        this.effect = EffectUtil.Instance.Play(this.m_Id, this.transform);
    }

    public void Stop()
    {
        if (this.effect != null)
        {
            EffectUtil.Instance.Stop(this.effect);
        }

        this.effect = null;
    }

}
