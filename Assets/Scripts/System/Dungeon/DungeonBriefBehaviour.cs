//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonBriefBehaviour : UIBase
{
    [SerializeField] TextEx m_DungeonName;
    [SerializeField] TextEx m_RecommenLevel;
    [SerializeField] TextEx m_SurplusTimes;
    [SerializeField] TextEx m_Introduce;
    [SerializeField] TextEx m_TimeLimit;
    [SerializeField] ItemGroupBehaviour m_Rewards;

    public void Display(int dungeonId)
    {
        if (!DungeonConfig.Has(dungeonId))
        {
            return;
        }

        DisplayBaseInfo(dungeonId);
        DisplaySurplusTimes(dungeonId);
    }

    private void DisplayBaseInfo(int dungeonId)
    {
        var config = DungeonConfig.Get(dungeonId);
        m_DungeonName.SetLanguage(config.name);
        m_Introduce.SetLanguage(config.description);
        m_TimeLimit.SetText(config.timeLimit);
        m_RecommenLevel.SetText(StringUtil.Contact(config.recommendLevel.x, "--", config.recommendLevel.y));

        var items = new List<Item>();
        foreach (var reward in config.rewards)
        {
            items.Add(new Item()
            {
                id = reward.x,
                count = reward.y
            });
        }

        m_Rewards.Display(items);
    }

    public void DisplaySurplusTimes(int dungeonId)
    {
        var config = DungeonConfig.Get(dungeonId);
        var surplusTimes = 0;
        if (config.dailyTimes > 0)
        {
            surplusTimes = Dungeon.Instance.GetDailySurplusTimes(dungeonId);
        }
        else
        {
            surplusTimes = Dungeon.Instance.GetWeekSurplusTimes(dungeonId);
        }
        m_SurplusTimes.SetText(surplusTimes);
    }




}



