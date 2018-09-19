using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBriefInfoWidget : Widget
{
    [SerializeField] TextEx m_PlayerName;
    [SerializeField] TextEx m_Level;
    [SerializeField] ImageEx m_Icon;
    [SerializeField] SmoothSlider m_Life;

    private void DisplayHp()
    {
        var info = PlayerInfo.Instance.GetPlayerHp();
        this.m_Life.value = info.x / (float)info.y;
    }

    private void DisplayPlayerBriefInfo()
    {
        var info = PlayerInfo.Instance.GetPlayerBriefInfo();
        this.m_PlayerName.SetText(info.name);
        this.m_Level.SetText(info.level);
        this.m_Icon.SetSprite(1);
    }


}
