using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FightActor : ActorBase
{

    public FightActor(int instanceId, ActorType type, Transform transform)
        : base(instanceId, type, transform)
    {
        if (transform == null)
        {
            throw new ArgumentNullException("transform is null");
        }
    }

    public int GetFightProperty(FightProperty property)
    {
        return propertyController.GetProperty(property);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnLateUpdate1()
    {
        base.OnLateUpdate1();
    }

    public override void OnLateUpdate2()
    {
        base.OnLateUpdate2();
    }

    public override void OnUpdate1()
    {
        base.OnUpdate1();
    }

    public override void OnUpdate2()
    {
        base.OnUpdate2();
    }

    public virtual void Attack(int attackId)
    {
        PushCommand(CommandType.Attack, attackId);
    }

    public virtual void CastSkill(int skillId)
    {
        PushCommand(CommandType.Skill, skillId);
    }

    public virtual void Hurt(HurtInfo info)
    {

    }

    public virtual void Dead()
    {

    }

    public virtual void ProcessAttackEvent(int attackIndex, int rangeLeft, int rangeRight)
    {

    }

    public virtual void ProcessSkillEvent(int skillIndex, int rangeLeft, int rangeRight)
    {

    }

}


