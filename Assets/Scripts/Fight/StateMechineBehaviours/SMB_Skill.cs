using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Skill : SMB_Base
{
    [SerializeField] int m_SkillIndex;

    FightActor fightActor;

    protected override void OnEnter(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnEnter(owner, animator, stateInfo, layerIndex);

        if (actor != null)
        {
            fightActor = actor as FightActor;
        }
    }

    protected override void OnUpdate(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnUpdate(owner, animator, stateInfo, layerIndex);
    }

    protected override void OnExit(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnExit(owner, animator, stateInfo, layerIndex);
    }

    protected override void OnProcessFrameEvent()
    {
        base.OnProcessFrameEvent();

        fightActor.CastSkill(m_SkillIndex);
    }


}
