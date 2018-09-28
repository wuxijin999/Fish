//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WorldBossBriefBehaviour : ScrollBehaviour
{

    [SerializeField] TextEx m_BossName;
    [SerializeField] TextEx m_Level;
    [SerializeField] ImageEx m_Icon;
    [SerializeField] ButtonEx m_SelectButton;
    [SerializeField] RectTransform m_SelectedBehaviour;

    WorldBoss.BossBrief bossBrief;

    public override void Display(object data)
    {
        base.Display(data);

        var bossId = (int)data;
        bossBrief = WorldBoss.Instance.GetBossBrief(bossId);
        DisplayBaseInfo();

        m_SelectButton.SetListener(SelectBoss);
        m_Icon.gray = bossBrief.rebornTime.Fetch() > DateTime.Now;
        m_SelectedBehaviour.SetActive(bossBrief.selected.Fetch());
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        DisplayDynamicInfo();
    }

    private void DisplayBaseInfo()
    {
        var config = WorldBossConfig.Get(bossBrief.bossId);
        m_BossName.SetText(config.name);
        m_Level.SetText(config.level);
        m_Icon.SetSprite(config.icon);
    }

    private void DisplayDynamicInfo()
    {
        if (bossBrief.rebornTime.dirty)
        {
            var dead = bossBrief.rebornTime.Fetch() > DateTime.Now;
            m_Icon.gray = dead;
        }

        if (bossBrief.selected.dirty)
        {
            m_SelectedBehaviour.SetActive(bossBrief.selected.Fetch());
        }
    }

    private void SelectBoss()
    {
        WorldBoss.Instance.SelectBoss(bossBrief.bossId);
    }

}



