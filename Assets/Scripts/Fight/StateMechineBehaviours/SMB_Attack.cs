using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Attack : SMB_Base
{

    [SerializeField] int m_AttackIndex = 0;

    EffectBehaviour effectBehaviour;

    protected override void OnEnter(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnEnter(owner, animator, stateInfo, layerIndex);

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

    }


}
