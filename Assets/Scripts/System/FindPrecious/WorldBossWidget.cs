//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldBossWidget : Widget
{

    [SerializeField] ButtonEx m_Goto;
    [SerializeField] TextEx m_KillTimes;
    [SerializeField] Toggle m_AttenToggle;
    [SerializeField] CyclicScroll m_BossInfoes;

    protected override void SetListeners()
    {
        base.SetListeners();
        m_Goto.SetListener(GotoKillBoss);
    }

    protected override void OnActived()
    {
        base.OnActived();

        WorldBoss.Instance.SelectBoss(WorldBoss.Instance.GetRecommendBossId());

        DisplayAtten();
        DisplayKillTimes();
        m_BossInfoes.Init(WorldBoss.Instance.GetBosses());
    }

    private void GotoKillBoss()
    {
        WorldBoss.Instance.GotoKillBoss(WorldBoss.Instance.selectedBoss.value);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (WorldBoss.Instance.killedTimes.dirty || WorldBoss.Instance.totalTimes.dirty)
        {
            DisplayKillTimes();
        }

        if (WorldBoss.Instance.selectedBoss.dirty)
        {
            DisplayAtten();
        }
    }

    private void DisplayKillTimes()
    {
        var killTimes = WorldBoss.Instance.killedTimes.Fetch();
        var totalTimes = WorldBoss.Instance.totalTimes.Fetch();

        m_KillTimes.SetText(StringUtil.Contact(killTimes, "/", totalTimes));
        m_KillTimes.SetColor(killTimes >= totalTimes ? ColorUtil.red : ColorUtil.green);
    }

    private void DisplayAtten()
    {
        var bossBrief = WorldBoss.Instance.GetBossBrief(WorldBoss.Instance.selectedBoss.value);
        m_AttenToggle.isOn = bossBrief.subscribed.value;
    }

}



