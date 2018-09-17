//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 14, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Presenter<PlayerInfo>
{

    public override void OpenWindow()
    {
    }

    public override void CloseWindow()
    {
    }


    public PlayerBriefInfo GetPlayerBriefInfo()
    {
        return new PlayerBriefInfo()
        {
            name = PlayerModel.Instance.playerName,
            level = PlayerModel.Instance.level,
            icon = "null",
        };
    }

    public Int2 GetPlayerHp()
    {
        return new Int2(PlayerModel.Instance.hp, PlayerModel.Instance.maxHp);
    }

    public struct PlayerBriefInfo
    {
        public string name;
        public string icon;
        public int level;
    }


}





