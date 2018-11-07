using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectPlay : MonoBehaviour
{

    [SerializeField] int m_Id;

    int effectInstanceId = 0;

    public void Play()
    {
        if (this.effectInstanceId != 0)
        {
            EffectUtil.Instance.Stop(this.effectInstanceId);
        }

        this.effectInstanceId = EffectUtil.Instance.Play(this.m_Id, this.transform);
    }

    public void Stop()
    {
        if (this.effectInstanceId != 0)
        {
            EffectUtil.Instance.Stop(this.effectInstanceId);
            this.effectInstanceId = 0;
        }
    }

}
