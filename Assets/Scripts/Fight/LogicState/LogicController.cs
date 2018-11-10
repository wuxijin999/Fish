using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicController
{
    public static readonly int Param_ActorInstanceId = Animator.StringToHash("ActorInstanceId");
    public static readonly float Move_Speed = Animator.StringToHash("MoveSpeed");
    public static readonly Dictionary<ActionType, int> stateHashs = new Dictionary<ActionType, int>()
        {
            {  ActionType.Idle,Animator.StringToHash("Idle") },
            {  ActionType.Move,Animator.StringToHash("Move") },
            {  ActionType.Jump,Animator.StringToHash("Jump") },
            {  ActionType.Dance,Animator.StringToHash("Dance") },
            {  ActionType.Attack1,Animator.StringToHash("Attack_1") },
            {  ActionType.Attack2,Animator.StringToHash("Attack_2") },
            {  ActionType.Attack3,Animator.StringToHash("Attack_3") },
            {  ActionType.Attack4,Animator.StringToHash("Attack_4") },
            {  ActionType.Skill1,Animator.StringToHash("Skill_1") },
            {  ActionType.Skill2,Animator.StringToHash("Skill_2") },
            {  ActionType.Skill3,Animator.StringToHash("Skill_3") },
            {  ActionType.Skill4,Animator.StringToHash("Skill_4") },
            {  ActionType.Skill_Special,Animator.StringToHash("Skill_Special") },
            {  ActionType.Dead,Animator.StringToHash("Dead") },
            {  ActionType.Hurt,Animator.StringToHash("Hurt") },
            {  ActionType.HurtDown,Animator.StringToHash("HurtDown") },
            {  ActionType.Stun,Animator.StringToHash("Stun") },
        };

    ActorBase actor;
    Dictionary<LogicStateType, LogicState_Base> states = new Dictionary<LogicStateType, LogicState_Base>();
    public LogicStateType currentState { get; private set; }

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

    AnimatorOverrideController overrideController = null;

    Animator m_Animator;
    public Animator animator {
        get { return this.m_Animator; }
        set {
            if (this.m_Animator != value)
            {
                this.m_Animator = value;
                this.overrideController = new AnimatorOverrideController(this.m_Animator.runtimeAnimatorController);
                this.m_Animator.runtimeAnimatorController = this.overrideController;
            }
        }
    }

    public bool isIntransition {
        get { return this.animator != null && this.animator.IsInTransition(0); }
    }

    public LogicController(ActorBase actor)
    {
        this.actor = actor;
        this.animator = this.actor.transform.GetComponent<Animator>();
        EnterState(LogicStateType.Idle, null);
    }

    public bool DoAction(LogicStateType stateType, object value = null)
    {
        if (Transitable(currentState, stateType))
        {
            if (states.ContainsKey(this.currentState))
            {
                states[currentState].Exit();
            }

            EnterState(stateType, value);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Update()
    {
        this.states[this.currentState].Update();
        if (stateCompleted)
        {
            EnterState(LogicStateType.Idle, null);
        }
    }

    void EnterState(LogicStateType stateType, object value)
    {
        this.currentState = stateType;
        var state = states.ContainsKey(stateType) ? states[stateType] : states[stateType] = CreateLogicState(stateType);
        state.Enter(value);
    }

    private LogicState_Base CreateLogicState(LogicStateType state)
    {
        switch (state)
        {
            case LogicStateType.Idle:
                return new LogicState_Idle(this.animator);
            case LogicStateType.Move:
                return new LogicState_Move(this.animator);
            case LogicStateType.Jump:
                return new LogicState_Jump(this.animator);
            case LogicStateType.Dance:
                return new LogicState_Dance(this.animator);
            case LogicStateType.Attack1:
                return new LogicState_Attack_1(this.animator);
            case LogicStateType.Attack2:
                return new LogicState_Attack_2(this.animator);
            case LogicStateType.Attack3:
                return new LogicState_Attack_3(this.animator);
            case LogicStateType.Attack4:
                return new LogicState_Attack_4(this.animator);
            case LogicStateType.Skill1:
                return new LogicState_Skill_1(this.animator);
            case LogicStateType.Skill2:
                return new LogicState_Skill_2(this.animator);
            case LogicStateType.Skill3:
                return new LogicState_Skill_3(this.animator);
            case LogicStateType.Skill4:
                return new LogicState_Skill_4(this.animator);
            case LogicStateType.Skill_Special:
                return new LogicState_Skill_Special(this.animator);
            case LogicStateType.Dead:
                return new LogicState_Dead(this.animator);
            case LogicStateType.Hurt:
                return new LogicState_Hurt(this.animator);
            case LogicStateType.HurtDown:
                return new LogicState_HurtDown(this.animator);
            case LogicStateType.Stun:
                return new LogicState_Stun(this.animator);
            default:
                return null;
        }
    }

    public void SetStateAnimationClip(string stateName, AnimationClip clip)
    {
        if (this.overrideController != null)
        {
            this.overrideController[stateName] = clip;
        }
    }

    private bool IsLoopState(LogicStateType actionState)
    {
        switch (actionState)
        {
            case LogicStateType.Idle:
            case LogicStateType.Move:
            case LogicStateType.Dead:
            case LogicStateType.Stun:
                return true;
            default:
                return false;
        }
    }

    private bool Transitable(LogicStateType from, LogicStateType to)
    {
        if (to == LogicStateType.Idle)
        {
            return true;
        }
        else if ((int)from > 1000 && (int)to < 1000)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

public enum LogicStateType
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
    Skill_Special = 205,

    Dead = 1001,
    Hurt = 1002,
    HurtDown = 1003,
    Stun = 1004,
}

public enum ActionType
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
    Skill_Special = 205,

    Dead = 1001,
    Hurt = 1002,
    HurtDown = 1003,
    Stun = 1004,
}



