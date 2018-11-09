using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Move : SMB_Base
{

    protected override void OnEnter(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnEnter(owner, animator, stateInfo, layerIndex);
    }

    protected override void OnUpdate(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnUpdate(owner, animator, stateInfo, layerIndex);

        var moveState = 1f;
        if (moveState < 0.25f)
        {
            //停的状态
        }
        else if (moveState < 0.75f)
        {
            //走的状态
            EffectUtil.Instance.Play(2, owner.transform);
        }
        else
        {
            //跑的状态
            EffectUtil.Instance.Play(3, owner.transform);
        }
    }

    protected override void OnExit(ActorBase owner, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnExit(owner, animator, stateInfo, layerIndex);

    }

}


