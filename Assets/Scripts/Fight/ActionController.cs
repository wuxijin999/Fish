using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class ActionController
    {
        public static readonly int Param_Action = Animator.StringToHash("Action");
        public static readonly int Param_ActorInstanceId = Animator.StringToHash("ActorInstanceId");
        public static readonly int Param_MoveState = Animator.StringToHash("MoveState");

        ActionState m_State = ActionState.CombatIdle;
        public ActionState state {
            get { return this.m_State; }
            set {
                if (this.m_State != value)
                {
                    this.m_State = value;
                    if (this.animator != null)
                    {
                        this.animator.SetTrigger((int)this.m_State);
                    }
                }
            }
        }

        Animator m_Animator;
        public Animator animator {
            get { return this.m_Animator; }
            set {
                if (this.m_Animator != value)
                {
                    this.m_Animator = value;
                    this.overrideController = new AnimatorOverrideController(this.m_Animator.runtimeAnimatorController);
                    this.m_Animator.runtimeAnimatorController = this.overrideController;
                    if (this.m_Animator != null)
                    {
                        this.m_Animator.SetTrigger((int)this.state);
                    }
                }
            }
        }

        AnimatorOverrideController overrideController = null;

        public bool isIntransition {
            get { return this.animator != null && this.animator.IsInTransition(0); }
        }

        public void SetStateAnimationClip(string stateName, AnimationClip clip)
        {
            if (this.overrideController != null)
            {
                this.overrideController[stateName] = clip;
            }
        }

    }


}
