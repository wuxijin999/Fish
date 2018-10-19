using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class PropertyController
    {
        public int maxHp { get; private set; }
        public int hp { get; private set; }
        public int minAttack { get; private set; }
        public int maxAttack { get; private set; }
        public int defense { get; private set; }
        public int hit { get; private set; }
        public int moveSpeed { get; private set; }
        public int attackSpeed { get; private set; }
        public int crit { get; private set; }
        public int haste { get; private set; }
        public int proficiency { get; private set; }

        public void SetProperty(FightProperty property, int value)
        {
            switch (property)
            {
                case FightProperty.MaxHp:
                    maxHp = value;
                    break;
                case FightProperty.Hp:
                    hp = value;
                    break;
                case FightProperty.MinAttack:
                    minAttack = value;
                    break;
                case FightProperty.MaxAttack:
                    maxAttack = value;
                    break;
                case FightProperty.Defense:
                    defense = value;
                    break;
                case FightProperty.Hit:
                    hit = value;
                    break;
                case FightProperty.MoveSpeed:
                    moveSpeed = value;
                    break;
                case FightProperty.AttackSpeed:
                    attackSpeed = value;
                    break;
                case FightProperty.Crit:
                    crit = value;
                    break;
                case FightProperty.Haste:
                    haste = value;
                    break;
                case FightProperty.Proficiency:
                    proficiency = value;
                    break;
                default:
                    break;
            }
        }

        public int GetProperty(FightProperty property)
        {
            switch (property)
            {
                case FightProperty.MaxHp:
                    return maxHp;
                case FightProperty.Hp:
                    return hp;
                case FightProperty.MinAttack:
                    return minAttack;
                case FightProperty.MaxAttack:
                    return maxAttack;
                case FightProperty.Defense:
                    return defense;
                case FightProperty.Hit:
                    return hit;
                case FightProperty.MoveSpeed:
                    return moveSpeed;
                case FightProperty.AttackSpeed:
                    return attackSpeed;
                case FightProperty.Crit:
                    return crit;
                case FightProperty.Haste:
                    return haste;
                case FightProperty.Proficiency:
                    return proficiency;
                default:
                    return 0;
            }
        }

        public float GetHitRate()
        {
            return Mathf.Clamp01(85 + hit * 0.01f);
        }

        public float GetAttackInterval()
        {
            return Mathf.Clamp(1 / (attackSpeed * 0.01f), 0.5f, 10f);
        }

        public float GetCritRate()
        {
            return crit * 0.003f;
        }

        public float GetHasteRate()
        {
            return haste * 0.002f;
        }

        public float GetProficiencyRate()
        {
            return proficiency * 0.001f;
        }

    }

    public enum FightProperty
    {
        MinAttack,
        MaxAttack,
        MaxHp,
        Hp,
        Defense,
        Hit,
        MoveSpeed,
        AttackSpeed,
        Crit,
        Haste,
        Proficiency,
    }

}

