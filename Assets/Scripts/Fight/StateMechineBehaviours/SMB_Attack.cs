using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class SMB_Attack : SMB_Base
    {

        [SerializeField] int m_AttackIndex = 0;

        FightActor fightActor;
        EffectBehaviour effect;

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
            this.fightActor = null;
            EffectUtil.Instance.Stop(this.effect);
            this.effect = null;
            base.OnExit(owner, animator, stateInfo, layerIndex);
        }

        protected override void OnProcessFrameEvent()
        {
            base.OnProcessFrameEvent();
            this.fightActor.ProcessAttackEvent(this.m_AttackIndex, this.processedFrame, this.frame);
        }


    }
}

