using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController
{
    public static readonly int Param_ActorInstanceId = Animator.StringToHash("ActorInstanceId");
    public static readonly float Move_Speed = Animator.StringToHash("MoveSpeed");

    static readonly Dictionary<ActionStateType, int> stateHashs = new Dictionary<ActionStateType, int>()
        {
            {  ActionStateType.Idle,Animator.StringToHash("Idle") },
            {  ActionStateType.Move,Animator.StringToHash("Move") },
            {  ActionStateType.Jump,Animator.StringToHash("Jump") },
            {  ActionStateType.Dance,Animator.StringToHash("Dance") },
            {  ActionStateType.Attack1,Animator.StringToHash("Attack_1") },
            {  ActionStateType.Attack2,Animator.StringToHash("Attack_2") },
            {  ActionStateType.Attack3,Animator.StringToHash("Attack_3") },
            {  ActionStateType.Attack4,Animator.StringToHash("Attack_4") },
            {  ActionStateType.Skill1,Animator.StringToHash("Skill_1") },
            {  ActionStateType.Skill2,Animator.StringToHash("Skill_2") },
            {  ActionStateType.Skill3,Animator.StringToHash("Skill_3") },
            {  ActionStateType.Skill4,Animator.StringToHash("Skill_4") },
            {  ActionStateType.Skill5,Animator.StringToHash("Skill_Special") },
            {  ActionStateType.Dead,Animator.StringToHash("Dead") },
            {  ActionStateType.Hurt,Animator.StringToHash("Hurt") },
            {  ActionStateType.HurtDown,Animator.StringToHash("HurtDown") },
            {  ActionStateType.Stun,Animator.StringToHash("Stun") },
        };

    ActionStateType m_StateType = ActionStateType.Idle;
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
        if (Transitable(this.m_StateType, state))
        {
            this.m_StateType = state;
            if (this.animator != null)
            {
                this.animator.SetBool(stateHashs[this.m_StateType], true);
            }
            return true;
        }
        else
        {
            return false;
        }
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
            case ActionStateType.Move:
            case ActionStateType.Dead:
            case ActionStateType.Stun:
                return true;
            default:
                return false;
        }
    }

    private bool Transitable(ActionStateType from, ActionStateType to)
    {
        if ((int)from > 1000 && (int)to < 1000)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}

public enum ActionStateType
{
    Idle = 1,
    Move = 2,
    Jump = 3,
    Dance = 4,

    Attack1 = 101,
    Attack2 = 102,
    Attack3 = 103,
    Attack4 = 104,
    Skill1 = 201,
    Skill2 = 202,
    Skill3 = 203,
    Skill4 = 204,
    Skill5 = 205,

    Dead = 1001,
    Hurt = 1002,
    HurtDown = 1003,
    Stun = 1004,
}

