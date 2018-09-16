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
                return playerModel.id;
            case PropertyType.Level:
                return playerModel.level;
            case PropertyType.Exp:
                return playerModel.exp;
            case PropertyType.Job:
                return playerModel.job;
            case PropertyType.MinAttack:
                return playerModel.minAttack;
            case PropertyType.MaxAttack:
                return playerModel.maxAttack;
            case PropertyType.Hp:
                return playerModel.hp;
            case PropertyType.Defense:
                return playerModel.defense;
            case PropertyType.Hit:
                return playerModel.hit;
            case PropertyType.MoveSpeed:
                return playerModel.moveSpeed;
            case PropertyType.Crit:
                return playerModel.crit;
            case PropertyType.Haste:
                return playerModel.haste;
            case PropertyType.Proficiency:
                return playerModel.proficiency;
            default:
                return 0;
        }
    }

    public string GetStringProperty(PropertyType type)
    {
        switch (type)
        {
            case PropertyType.PlayerName:
                return playerModel.playerName;
            default:
                return string.Empty;
        }
    }

    public void SetProperty(PropertyType type, int value)
    {
        playerModel.UpdateProperty(type, value);
        propertyEvent.Invoke(type);
    }

    public void SetProperty(PropertyType type, string value)
    {
        playerModel.UpdateProperty(type, value);
        propertyEvent.Invoke(type);
    }

}





