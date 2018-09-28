//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossModelShow : UIBase
{

    [SerializeField] RawImage m_BossPhoto;
    [SerializeField] TextEx m_BossName;
    [SerializeField] TextEx m_BossLevel;
    [SerializeField] TextEx m_BossDescription;

    int bossId = 0;
    Clock delayClock;

    public void Show(int bossId, float delay = 0f)
    {
        this.bossId = bossId;

        if (delay == 0f)
        {
            DisplayBaseInfo();
        }
        else
        {
            if (delayClock != null && !delayClock.end)
            {
                delayClock.Stop();
            }

            Clock.ClockParams setting = new Clock.ClockParams()
            {
                type = Clock.ClockType.UnityTimeClock,
                second = 0.3f,
            };

            delayClock = ClockUtil.Instance.Create(setting, OnDelayShow);
        }
    }

    public void Dispose()
    {
        if (delayClock != null && !delayClock.end)
        {
            delayClock.Stop();
        }
    }

    private void DisplayBaseInfo()
    {
        var config = NpcConfig.Get(bossId);
        m_BossName.SetText(config.name);
        m_BossLevel.SetText(config.level);

        var worldBossConfig = WorldBossConfig.Get(bossId);
        m_BossDescription.SetText(worldBossConfig.name);
    }

    private void OnDelayShow()
    {
        DisplayBaseInfo();
    }

}



