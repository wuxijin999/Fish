//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Sunday, September 16, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Presenter<PlayerInfo>
{
    PlayerModel playerModel = new PlayerModel();
    public BizEvent<PropertyType> propertyEvent = new BizEvent<PropertyType>();

    public override void OpenWindow()
    {
    }

    public override void CloseWindow()
    {
    }

    public int GetIntProperty(PropertyType type)
    {
        switch (type)
        {
            case PropertyType.PlayerId:
                return this.playerModel.id;
            case PropertyType.Level:
                return this.playerModel.level;
            case PropertyType.Exp:
                return this.playerModel.exp;
            case PropertyType.Job:
                return this.playerModel.job;
            case PropertyType.MinAttack:
                return this.playerModel.minAttack;
            case PropertyType.MaxAttack:
                return this.playerModel.maxAttack;
            case PropertyType.Hp:
                return this.playerModel.hp;
            case PropertyType.Defense:
                return this.playerModel.defense;
            case PropertyType.Hit:
                return this.playerModel.hit;
            case PropertyType.MoveSpeed:
                return this.playerModel.moveSpeed;
            case PropertyType.Crit:
                return this.playerModel.crit;
            case PropertyType.Haste:
                return this.playerModel.haste;
            case PropertyType.Proficiency:
                return this.playerModel.proficiency;
            default:
                return 0;
        }
    }

    public string GetStringProperty(PropertyType type)
    {
        switch (type)
        {
            case PropertyType.PlayerName:
                return this.playerModel.playerName;
            default:
                return string.Empty;
        }
    }

    public void SetProperty(PropertyType type, int value)
    {
        this.playerModel.UpdateProperty(type, value);
        this.propertyEvent.Invoke(type);
    }

    public void SetProperty(PropertyType type, string value)
    {
        this.playerModel.UpdateProperty(type, value);
        this.propertyEvent.Invoke(type);
    }

    public PlayerBriefInfo GetPlayerBriefInfo()
    {
        return new PlayerBriefInfo()
        {
            name = this.playerModel.playerName,
            level = this.playerModel.level,
            icon = 0,
        };
    }

    public Int2 GetPlayerHp()
    {
        return new Int2(this.playerModel.hp, this.playerModel.maxHp);
    }

    public struct PlayerBriefInfo
    {
        public string name;
        public int icon;
        public int level;
    }
}





