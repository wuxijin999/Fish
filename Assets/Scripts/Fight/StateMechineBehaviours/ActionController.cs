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
                        this.animator.SetInteger(Param_Action, (int)this.m_State);
                    }
                }
            }
        }

        public bool stateCompleted {
            get {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.loop)
                {
                    return false;
                }
                else
                {
                    return stateInfo.normalizedTime > 0.95f;
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

        public void Update()
        {
        }

        private bool IsLoopState(ActionState actionState)
        {
            switch (actionState)
            {
                case ActionState.Idle:
                case ActionState.CombatIdle:
                case ActionState.Walk:
                case ActionState.Run:
                case ActionState.Dead:
                    return true;
                default:
                    return false;
            }

        }

    }

    public enum ActionState
    {
        Idle = 1,
        CombatIdle = 2,
        Walk = 3,
        Run = 4,
        Jump = 5,
        Dead = 6,
        Dance = 7,

        Attack1 = 101,
        Attack2 = 102,
        Attack3 = 103,
        Attack4 = 104,
        Skill1 = 201,
        Skill2 = 202,
        Skill3 = 203,
        Skill4 = 204,
        Skill5 = 205,

        Hurt = 1001,
        HurtDown = 1002,
        Stun = 1004,
    }

}
