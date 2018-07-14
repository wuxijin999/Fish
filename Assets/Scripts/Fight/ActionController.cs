using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController
{

    ActionState m_State = ActionState.CombatIdle;
    public ActionState state
    {
        get { return m_State; }
        set
        {
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
    public Animator animator
    {
        get { return m_Animator; }
        set
        {
            if (m_Animator != value)
            {
                m_Animator = value;
                if (m_Animator != null)
                {
                    m_Animator.SetTrigger((int)state);
                }
            }
        }
    }

    public bool isIntransition
    {
        get { return animator != null && animator.IsInTransition(0); }
    }

}

