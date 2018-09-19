using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class SMB_Base : StateMachineBehaviour
    {
        protected ActorBase actor;
        protected int frame = 0;
        protected int processedFrame = -1;

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            this.frame = 0;
            this.processedFrame = -1;
            var instanceId = animator.GetInteger(ActionController.Param_ActorInstanceId);
            if (ActorCenter.Instance.TryGet(instanceId, out this.actor))
            {
                OnEnter(this.actor, animator, stateInfo, layerIndex);
            }
        }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            var normalizedInt = (int)stateInfo.normalizedTime;
            this.frame = (int)((stateInfo.normalizedTime - normalizedInt) * stateInfo.length /** GlobalDefine.ANIMATION_FRAMERATE*/);

            if (this.frame > this.processedFrame)
            {
                try
                {
                    OnProcessFrameEvent();
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex);
                }
                finally
                {
                    this.processedFrame = this.frame;
                }
            }

            if (this.actor != null)
            {
                OnUpdate(this.actor, animator, stateInfo, layerIndex);
            }
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            this.frame = 0;
            this.processedFrame = -1;
            if (this.actor != null)
            {
                OnExit(this.actor, animator, stateInfo, layerIndex);
                this.actor = null;
            }
        }

        protected virtual void OnEnter(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        protected virtual void OnUpdate(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        protected virtual void OnExit(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        protected virtual void OnProcessFrameEvent()
        {

        }

    }
}

