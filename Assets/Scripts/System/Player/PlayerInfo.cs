//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Sunday, September 16, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Presenter<PlayerInfo>
{
    PlayerModel model = new PlayerModel();
    public BizEvent<PropertyType> propertyEvent = new BizEvent<PropertyType>();

    public int GetIntProperty(PropertyType type)
    {
        switch (type)
        {
            case PropertyType.PlayerId:
                return this.model.id;
            case PropertyType.Level:
                return this.model.level;
            case PropertyType.Exp:
                return this.model.exp;
            case PropertyType.Job:
                return this.model.job;
            case PropertyType.MinAttack:
                return this.model.minAttack;
            case PropertyType.MaxAttack:
                return this.model.maxAttack;
            case PropertyType.Hp:
                return this.model.hp;
            case PropertyType.Defense:
                return this.model.defense;
            case PropertyType.Hit:
                return this.model.hit;
            case PropertyType.MoveSpeed:
                return this.model.moveSpeed;
            case PropertyType.Crit:
                return this.model.crit;
            case PropertyType.Haste:
                return this.model.haste;
            case PropertyType.Proficiency:
                return this.model.proficiency;
            default:
                return 0;
        }
    }

    public string GetStringProperty(PropertyType type)
    {
        switch (type)
        {
            case PropertyType.PlayerName:
                return this.model.playerName;
            default:
                return string.Empty;
        }
    }

    public void SetProperty(PropertyType type, int value)
    {
        this.model.UpdateProperty(type, value);
        this.propertyEvent.Invoke(type);
    }

    public void SetProperty(PropertyType type, string value)
    {
        this.model.UpdateProperty(type, value);
        this.propertyEvent.Invoke(type);
    }

    public PlayerBriefInfo GetPlayerBriefInfo()
    {
        return new PlayerBriefInfo()
        {
            name = this.model.playerName,
            level = this.model.level,
            icon = 0,
        };
    }

    public PlayerAvatar GetPlayerAvatar()
    {
        return new PlayerAvatar()
        {
            cuirass = model.cuirass,
            weapon = model.weapon,
            offhand = model.offhand,
        };
    }

    public Int2 GetPlayerHp()
    {
        return new Int2(this.model.hp, this.model.maxHp);
    }

    public struct PlayerAvatar
    {
        public int cuirass;
        public int weapon;
        public int offhand;
    }

    public struct PlayerBriefInfo
    {
        public string name;
        public int icon;
        public int level;
    }

}





