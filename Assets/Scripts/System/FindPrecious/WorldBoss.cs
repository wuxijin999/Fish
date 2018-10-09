//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldBoss : Presenter<WorldBoss>, IPresenterInit
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

    public override void OpenWindow(int functionId = 0)
    {
    }

    public override void CloseWindow()
    {
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

    public void UpdateBossInfoes(List<int> bossIds, List<int> seconds, List<bool> subscribes)
    {
        for (var i = 0; i < bossIds.Count; i++)
        {
            var bossId = bossIds[i];
            this.model.UpdateBossInfo(bossId, seconds[i]);
            this.model.UpdateBossSubscribe(bossId, subscribes[i]);

            var brief = this.bossBriefs.ContainsKey(bossId) ? this.bossBriefs[i] : this.bossBriefs[i] = new BossBrief(bossId);

            WorldBossModel.Boss bossInfo;
            if (this.model.TryGetBossInfo(bossId, out bossInfo))
            {
                brief.rebornTime.value = bossInfo.rebornTime;
                brief.subscribed.value = bossInfo.subscribed;
            }
        }
    }

    public BossBrief GetBossBrief(int bossId)
    {
        return this.bossBriefs.ContainsKey(bossId) ? this.bossBriefs[bossId] : null;
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





