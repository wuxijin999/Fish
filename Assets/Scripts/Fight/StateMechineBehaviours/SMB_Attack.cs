using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class SMB_Attack : SMB_Base
    {

        [SerializeField] int m_AttackIndex = 0;

        FightActor fightActor;
        int effectInstanceId = 0;

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
            if (effectInstanceId != 0)
            {
                EffectUtil.Instance.Stop(effectInstanceId);
                this.effectInstanceId = 0;
            }
            base.OnExit(owner, animator, stateInfo, layerIndex);
        }

        protected override void OnProcessFrameEvent()
        {
            base.OnProcessFrameEvent();
            this.fightActor.ProcessAttackEvent(this.m_AttackIndex, this.processedFrame, this.frame);
        }


    }
}

