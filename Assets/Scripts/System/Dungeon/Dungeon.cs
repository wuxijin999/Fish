//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Presenter<Dungeon>
{
    DungeonModel model = new DungeonModel();

    public readonly IntProperty lastDungeonId = new IntProperty();
    public BizEvent<int> dungeonInfoUpdateEvent = new BizEvent<int>();

    public void UpdateDungeonInfo(int dungeoId)
    {

        dungeonInfoUpdateEvent.Invoke(dungeoId);
    }

    public void EneterDungeon(int dungeonId)
    {
        if (!DungeonConfig.Has(dungeonId))
        {
            DebugEx.LogFormat("查无此副本，别开玩笑了:{0}", dungeonId);
            return;
        }

        var config = DungeonConfig.Get(dungeonId);
    }

    public void EneterDungeonGroup(int dungeonId)
    {
        if (!DungeonConfig.Has(dungeonId))
        {
            DebugEx.LogFormat("查无此副本，别开玩笑了:{0}", dungeonId);
            return;
        }

        var config = DungeonConfig.Get(dungeonId);
    }

    public void ExitDungeon()
    {

    }

    public int GetDungeonHighestStar(int dungeonId)
    {
        var star = 0;
        DungeonModel.Dungeon dungeon;
        if (model.TryGetDungeon(dungeonId, out dungeon))
        {
            star = dungeon.highestStar;
        }

        return star;
    }

    public int GetEnterTimes(int dungeonId)
    {
        var times = 0;
        DungeonModel.Dungeon dungeon;
        if (model.TryGetDungeon(dungeonId, out dungeon))
        {
            times = dungeon.enterTimes;
        }

        return times;
    }

    public int GetDailyTotalTimes(int dungeonId)
    {
        if (DungeonConfig.Has(dungeonId))
        {
            return DungeonConfig.Get(dungeonId).dailyTimes;
        }
        else
        {
            return 0;
        }
    }

    public int GetWeekTotalTimes(int dungeonId)
    {
        if (DungeonConfig.Has(dungeonId))
        {
            return DungeonConfig.Get(dungeonId).weekTimes;
        }
        else
        {
            return 0;
        }
    }

    public int GetDailySurplusTimes(int dungeonId)
    {
        if (DungeonConfig.Has(dungeonId))
        {
            return DungeonConfig.Get(dungeonId).dailyTimes - GetEnterTimes(dungeonId);
        }
        else
        {
            return 0;
        }
    }

    public int GetWeekSurplusTimes(int dungeonId)
    {
        if (DungeonConfig.Has(dungeonId))
        {
            return DungeonConfig.Get(dungeonId).weekTimes - GetEnterTimes(dungeonId);
        }
        else
        {
            return 0;
        }
    }

}





