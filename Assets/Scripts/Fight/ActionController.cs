using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController
{
    public static readonly int Param_Action = Animator.StringToHash("Action");
    public static readonly int Param_ActorInstanceId = Animator.StringToHash("ActorInstanceId");
    public static readonly int Param_MoveState = Animator.StringToHash("MoveState");

    ActionState m_State = ActionState.CombatIdle;
    public ActionState state {
        get { return m_State; }
        set {
            if (m_State != value)
            {
                m_State = value;
                if (animator != null)
                {
                    animator.SetTrigger((int)m_State);
                }
            }
        }
    }

    Animator m_Animator;
    public Animator animator {
        get { return m_Animator; }
        set {
            if (m_Animator != value)
            {
                m_Animator = value;
                overrideController = new AnimatorOverrideController(m_Animator.runtimeAnimatorController);
                m_Animator.runtimeAnimatorController = overrideController;
                if (m_Animator != null)
                {
                    m_Animator.SetTrigger((int)state);
                }
            }
        }
    }

    AnimatorOverrideController overrideController = null;

    public bool isIntransition {
        get { return animator != null && animator.IsInTransition(0); }
    }

    public void SetStateAnimationClip(string stateName, AnimationClip clip)
    {
        if (overrideController != null)
        {
            overrideController[stateName] = clip;
        }
    }

}

