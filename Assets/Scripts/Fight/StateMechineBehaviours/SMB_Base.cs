using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Base : StateMachineBehaviour
{

    protected ActorBase actor;
    protected int frame = 0;
    protected int processedFrame = 0;

    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        frame = 0;
        processedFrame = 0;
        var instanceId = animator.GetInteger(ActionController.Param_ActorInstanceId);
        if (ActorCenter.Instance.TryGet(instanceId, out actor))
        {
            OnEnter(actor, animator, stateInfo, layerIndex);
        }
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        var normalizedInt = (int)stateInfo.normalizedTime;
        frame = (int)((stateInfo.normalizedTime - normalizedInt) * stateInfo.length * GlobalDefine.ANIMATION_FRAMERATE);

        if (frame > processedFrame)
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
                processedFrame = frame;
            }
        }

        if (actor != null)
        {
            OnUpdate(actor, animator, stateInfo, layerIndex);
        }
    }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        frame = 0;
        processedFrame = 0;
        if (actor != null)
        {
            OnExit(actor, animator, stateInfo, layerIndex);
            actor = null;
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
