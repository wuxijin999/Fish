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

        ActionStateType m_StateType = ActionStateType.CombatIdle;
        public ActionStateType stateType {
            get { return this.m_StateType; }
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
                        this.m_Animator.SetTrigger((int)this.stateType);
                    }
                }
            }
        }

        AnimatorOverrideController overrideController = null;

        public bool isIntransition {
            get { return this.animator != null && this.animator.IsInTransition(0); }
        }

        public bool EnterState(ActionStateType state)
        {
            this.m_StateType = state;
            if (this.animator != null)
            {
                this.animator.SetInteger(Param_Action, (int)this.m_StateType);
            }

            return true;
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

        private bool IsLoopState(ActionStateType actionState)
        {
            switch (actionState)
            {
                case ActionStateType.Idle:
                case ActionStateType.CombatIdle:
                case ActionStateType.Move:
                case ActionStateType.Dead:
                    return true;
                default:
                    return false;
            }
        }

    }

    public enum ActionStateType
    {
        Idle = 1,
        CombatIdle = 2,
        Move = 3,
        Jump = 4,
        Dead = 5,
        Dance = 6,

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
