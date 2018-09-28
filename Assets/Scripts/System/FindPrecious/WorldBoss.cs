//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldBoss : Presenter<WorldBoss>
{

    public readonly BaseProperty<int> killedTimes = new BaseProperty<int>(0);
    public readonly BaseProperty<int> totalTimes = new BaseProperty<int>(0);
    public readonly BaseProperty<int> selectedBoss = new BaseProperty<int>(0);

    Dictionary<int, BossBrief> bossBriefs = new Dictionary<int, BossBrief>();
    WorldBossModel model = new WorldBossModel();

    public override void OpenWindow(int functionId = 0)
    {
    }

    public override void CloseWindow()
    {

    }

    public void GotoKillBoss(int bossId)
    {

    }

    public void UpdateBossInfoes(List<int> bossIds, List<int> seconds, List<bool> subscribes)
    {
        for (var i = 0; i < bossIds.Count; i++)
        {
            model.UpdateBossInfo(bossIds[i], seconds[i]);
            model.UpdateBossSubscribe(bossIds[i], subscribes[i]);
        }
    }

    public BossBrief GetBossBrief(int bossId)
    {
        return bossBriefs.ContainsKey(bossId) ? bossBriefs[bossId] : null;
    }

    public class BossBrief
    {
        public int bossId;
        public BaseProperty<DateTime> rebornTime = new BaseProperty<DateTime>();
        public BaseProperty<bool> subscribed = new BaseProperty<bool>();
    }

}





