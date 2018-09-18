using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightActor : ActorBase
{


    public FightActor(Transform transform) : base(transform)
    {

    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    protected override void OnLateUpdate1()
    {
        base.OnLateUpdate1();
    }

    protected override void OnLateUpdate2()
    {
        base.OnLateUpdate2();
    }

    protected override void OnUpdate1()
    {
        base.OnUpdate1();
    }

    protected override void OnUpdate2()
    {
        base.OnUpdate2();
    }

    public int GetSkillId(string stateName)
    {

        return 0;
    }

    public void CastSkill(int skillIndex)
    {

    }

}
