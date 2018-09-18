using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{

    [SerializeField] bool m_Loop;
    [SerializeField] float m_Duration;

    int m_EffectId = 0;
    public int effectId { get { return m_EffectId; } set { m_EffectId = value; } }

    float stopTime = 0f;
    Animator[] animators;
    Animation[] animations;
    ParticleSystem[] particleSystems;

    Transform target;

    private void Awake()
    {
        animators = this.GetComponentsInChildren<Animator>(true);
        animations = this.GetComponentsInChildren<Animation>(true);
        particleSystems = this.GetComponentsInChildren<ParticleSystem>(true);
    }

    public void OnPlay(Transform target = null)
    {
        if (animators != null)
        {
            foreach (var animator in animators)
            {
                animator.enabled = true;
            }
        }

        if (animations != null)
        {
            foreach (var animation in animations)
            {
                animation.enabled = true;
            }
        }

        stopTime = Time.time + m_Duration;
        this.target = target;
    }

    public void OnStop()
    {
        if (animators != null)
        {
            foreach (var animator in animators)
            {
                animator.enabled = false;
            }
        }

        if (animations != null)
        {
            foreach (var animation in animations)
            {
                animation.enabled = false;
            }
        }

        target = null;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            this.transform.position = target.position;
            this.transform.rotation = target.rotation;
        }

        if (!m_Loop)
        {
            if (Time.time >= stopTime)
            {
                EffectUtil.Instance.Stop(this);
            }
        }
    }
}
