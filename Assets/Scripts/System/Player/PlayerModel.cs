//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Sunday, September 16, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel :Model
{
    public int id { get; private set; }
    public string playerName { get; private set; }
    public int level { get; private set; }
    public int exp { get; private set; }
    public int job { get; private set; }
    public int minAttack { get; private set; }
    public int maxAttack { get; private set; }
    public int maxHp { get; private set; }
    public int hp { get; private set; }
    public int defense { get; private set; }
    public int hit { get; private set; }
    public int moveSpeed { get; private set; }
    public int attackSpeed { get; private set; }
    public int crit { get; private set; }
    public int haste { get; private set; }
    public int proficiency { get; private set; }

    public override void Reset()
    {
    }

    public void UpdateProperty(PropertyType type, int value)
    {
        switch (type)
        {
            case PropertyType.PlayerId:
                this.id = value;
                break;
            case PropertyType.Level:
                this.level = value;
                break;
            case PropertyType.Exp:
                this.exp = value;
                break;
            case PropertyType.Job:
                this.job = value;
                break;
            case PropertyType.MinAttack:
                this.minAttack = value;
                break;
            case PropertyType.MaxAttack:
                this.maxAttack = value;
                break;
            case PropertyType.Hp:
                this.hp = value;
                break;
            case PropertyType.Defense:
                this.defense = value;
                break;
            case PropertyType.Hit:
                this.hit = value;
                break;
            case PropertyType.MoveSpeed:
                this.moveSpeed = value;
                break;
            case PropertyType.Crit:
                this.crit = value;
                break;
            case PropertyType.Haste:
                this.haste = value;
                break;
            case PropertyType.Proficiency:
                this.proficiency = value;
                break;
            default:
                break;
        }

    }

    public void UpdateProperty(PropertyType type, string value)
    {
        switch (type)
        {
            case PropertyType.PlayerName:
                this.playerName = value;
                break;
            default:
                break;
        }
    }

}

public enum PropertyType
{
    PlayerId,
    PlayerName,
    Level,
    Exp,
    Job,
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





