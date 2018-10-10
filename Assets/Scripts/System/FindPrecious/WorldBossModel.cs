//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldBossModel
{
    Dictionary<int, Boss> bosses = new Dictionary<int, Boss>();

    public void UpdateBossInfo(int bossId, int second)
    {
        var boss = GetBoss(bossId);
        boss.rebornTime = Time.realtimeSinceStartup + second;
    }

    public void UpdateBossSubscribe(int bossId, bool subscribed)
    {
        var boss = GetBoss(bossId);
        boss.subscribed = subscribed;
    }

    public bool TryGetBossInfo(int bossId, out Boss boss)
    {
        return bosses.TryGetValue(bossId, out boss);
    }

    private Boss GetBoss(int bossId)
    {
        return bosses.ContainsKey(bossId) ? bosses[bossId] : bosses[bossId] = new Boss();
    }

    public class Boss
    {
        public float rebornTime = 0f;
        public bool subscribed = false;
        public List<KillRecord> killrecords = new List<KillRecord>();

    }

    public struct KillRecord
    {
        public string killerName;
        public DateTime killTime;
    }

}





