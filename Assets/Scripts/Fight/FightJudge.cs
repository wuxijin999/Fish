using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class FightJudge : Singleton<FightJudge>
    {

        public void Attack(AttackInfo attackInfo)
        {
            ActorBase attackerBase;
            ActorCenter.Instance.TryGet(attackInfo.attacker, out attackerBase);
            var attacker = attackerBase as FightActor;

            if (attacker == null)
            {
                DebugEx.Log("没有攻击者，你攻击个啥呢！");
                return;
            }

            ActorBase hurterBase;
            ActorCenter.Instance.TryGet(attackInfo.attackTarget, out hurterBase);
            var target = hurterBase as FightActor;

            var error = 0;
            if (!CanAttack(target, attackInfo.skillId, out error))
            {
                return;
            }

            var hurtInfo = new HurtInfo();
            hurtInfo.attacker = attacker.instanceId;
            hurtInfo.hurtTarget = target.instanceId;
            hurtInfo.hurtTime = Time.realtimeSinceStartup;

            var minAttack = attacker.GetFightProperty(FightProperty.MinAttack);
            var maxAttack = attacker.GetFightProperty(FightProperty.MaxAttack);
            var attack = Random.Range(minAttack, maxAttack + 1);
            var critRate = attacker.GetFightProperty(FightProperty.Crit) * 10000;
            var isCrit = Random.Range(0, critRate) < critRate;
            var defense = target.GetFightProperty(FightProperty.Defense);
            var damage = attack * (1 + (isCrit ? 1 : 0)) - defense;

            hurtInfo.damage = damage;
            hurtInfo.damageType = isCrit ? DamageType.Crit : DamageType.Normal;
            hurtInfo.direction = Vector3.Normalize(target.transform.position - attackInfo.position);

            target.Hurt(hurtInfo);

            if (!IsAlive(target))
            {
                target.Dead();
            }
        }

        bool CanAttack(FightActor target, int skillId, out int error)
        {
            if (target != null)
            {
                error = 1;
                //如果是友军、或者无敌状态的对象，则不可攻击
                return true;
            }
            else
            {
                if (SkillConfig.Has(skillId))
                {
                    error = 2;
                    return false;
                }
            }

            error = 0;
            return true;
        }

        bool IsAlive(FightActor target)
        {
            if (target == null)
            {
                return false;
            }

            var hp = target.GetFightProperty(FightProperty.Hp);
            return hp > 0;
        }

    }

    public enum AttackType
    {
        Physical = 1,
        Magic = 2,
        Holy = 3,
    }

    public enum DamageType
    {
        Normal,
        Crit,
    }

    public struct AttackInfo
    {
        public int attacker;
        public int attackTarget;
        public int skillId;
        public float attackTime;
        public Vector3 position;
    }

    public struct HurtInfo
    {
        public int attacker;
        public int hurtTarget;
        public float hurtTime;
        public Vector3 direction;
        public DamageType damageType;
        public int damage;
    }

}

