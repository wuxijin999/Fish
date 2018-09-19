using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{

    [SerializeField] bool m_Loop;
    [SerializeField] float m_Duration;

    int m_EffectId = 0;
    public int effectId { get { return this.m_EffectId; } set { this.m_EffectId = value; } }

    float stopTime = 0f;
    Animator[] animators;
    Animation[] animations;
    ParticleSystem[] particleSystems;

    Transform target;

    private void Awake()
    {
        this.animators = this.GetComponentsInChildren<Animator>(true);
        this.animations = this.GetComponentsInChildren<Animation>(true);
        this.particleSystems = this.GetComponentsInChildren<ParticleSystem>(true);
    }

    public void OnPlay(Transform target = null)
    {
        if (this.animators != null)
        {
            foreach (var animator in this.animators)
            {
                animator.enabled = true;
            }
        }

        if (this.animations != null)
        {
            foreach (var animation in this.animations)
            {
                animation.enabled = true;
            }
        }

        this.stopTime = Time.time + this.m_Duration;
        this.target = target;
    }

    public void OnStop()
    {
        if (this.animators != null)
        {
            foreach (var animator in this.animators)
            {
                animator.enabled = false;
            }
        }

        if (this.animations != null)
        {
            foreach (var animation in this.animations)
            {
                animation.enabled = false;
            }
        }

        this.target = null;
    }

    private void LateUpdate()
    {
        if (this.target != null)
        {
            this.transform.position = this.target.position;
            this.transform.rotation = this.target.rotation;
        }

        if (!this.m_Loop)
        {
            if (Time.time >= this.stopTime)
            {
                EffectUtil.Instance.Stop(this);
            }
        }
    }
}
