//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldBoss : Presenter<WorldBoss>, IPresenterInit, IPresenterReset
{
    public readonly IntProperty killedTimes = new IntProperty(0);
    public readonly IntProperty totalTimes = new IntProperty(0);
    public readonly IntProperty selectedBoss = new IntProperty(0);

    List<int> sortedBossIds = new List<int>();
    Dictionary<int, BossBrief> bossBriefs = new Dictionary<int, BossBrief>();
    WorldBossModel model = new WorldBossModel();

    public void Init()
    {
        for (var i = 1; i <= 10; i++)
        {
            sortedBossIds.Add(i);
        }

        sortedBossIds.Sort(BossCompare);
    }

    public void OnReset()
    {
        model.Reset();
    }

    public void OpenWindow(int functionId = 0)
    {
    }

    public void CloseWindow()
    {
        bossBriefs.Clear();
    }

    public void GotoKillBoss(int bossId)
    {

    }

    public void SelectBoss(int bossId)
    {
        selectedBoss.value = bossId;
        foreach (var item in bossBriefs.Values)
        {
            item.selected.value = item.bossId == bossId;
        }
    }

    public void SubscribeBoss(int bossId)
    {
        if (this.bossBriefs.ContainsKey(bossId))
        {
            this.bossBriefs[bossId].subscribed.value = true;
        }

        model.UpdateBossSubscribe(bossId, true);
    }

    public void DesubscribeBoss(int bossId)
    {
        if (this.bossBriefs.ContainsKey(bossId))
        {
            this.bossBriefs[bossId].subscribed.value = false;
        }

        model.UpdateBossSubscribe(bossId, false);
    }

    public void UpdateBossInfoes(List<int> bossIds, List<int> seconds, List<bool> subscribes)
    {
        if (bossIds == null)
        {
            DebugEx.LogError("bossids is null");
            return;
        }

        if (seconds == null)
        {
            DebugEx.LogError("seconds is null ");
            return;
        }

        if (subscribes == null)
        {
            DebugEx.LogError("subscribes is null ");
            return;
        }

        for (var i = 0; i < bossIds.Count; i++)
        {
            var bossId = bossIds[i];
            this.model.UpdateBossInfo(bossId, seconds[i]);
            this.model.UpdateBossSubscribe(bossId, subscribes[i]);
        }
    }

    public BossBrief GetBossBrief(int bossId)
    {
        WorldBossModel.Boss bossInfo;
        if (model.TryGetBossInfo(bossId, out bossInfo))
        {
            BossBrief brief;
            if (this.bossBriefs.ContainsKey(bossId))
            {
                brief = this.bossBriefs[bossId];
            }
            else
            {
                brief = this.bossBriefs[bossId] = new BossBrief(bossId);
                brief.rebornTime.value = bossInfo.rebornTime;
                brief.subscribed.value = bossInfo.subscribed;
                brief.selected.value = bossId == selectedBoss.value;
            }

            return brief;
        }
        else
        {
            DebugEx.LogFormat("查无此boss：{0},不要忽悠老人家！");
            return null;
        }
    }

    public List<int> GetBosses()
    {
        return new List<int>(sortedBossIds);
    }

    public int GetRecommendBossId()
    {
        return sortedBossIds[0];
    }

    public class BossBrief
    {
        public readonly int bossId;
        public readonly FloatProperty rebornTime = new FloatProperty();
        public readonly BoolProperty subscribed = new BoolProperty();
        public readonly BoolProperty selected = new BoolProperty(false);

        public BossBrief(int bossId)
        {
            this.bossId = bossId;
        }
    }

    static int BossCompare(int lhs, int rhs)
    {
        return -WorldBossConfig.Get(lhs).level.CompareTo(WorldBossConfig.Get(rhs).level);
    }


}





