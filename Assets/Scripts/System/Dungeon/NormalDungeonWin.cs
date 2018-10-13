//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalDungeonWin : Window
{
    [SerializeField] DungeonBriefBehaviour m_DungeonInfo;
    [SerializeField] ButtonEx m_SingleChallenge;
    [SerializeField] ButtonEx m_TeamChallenge;
    [SerializeField] ButtonEx m_Sweep;
    [SerializeField] UI3DNpcDrawer m_BossModel;

    #region Built-in

    protected override void SetListeners()
    {
        setting.close.SetListener(() => { NormalDungeon.Instance.CloseWindow(); });
        m_SingleChallenge.SetListener(() => { NormalDungeon.Instance.EnterDungeon(); });
        m_TeamChallenge.SetListener(() => { NormalDungeon.Instance.EnterDungeonGroup(); });
        m_Sweep.SetListener(() => { NormalDungeon.Instance.Sweep(); });
    }

    protected override void OnPreOpen()
    {
        DisplayBaseInfo();
        DisplaySweepButton();
        DisplayChallengeState();
    }

    protected override void OnPreClose()
    {
    }

    public override void OnLateUpdate()
    {
        if (NormalDungeon.Instance.showSweep.dirty)
        {
            DisplaySweepButton();
        }

        if (NormalDungeon.Instance.surplurTime.dirty)
        {
            DisplayChallengeState();
        }
    }

    #endregion

    private void DisplayBaseInfo()
    {
        var config = DungeonConfig.Get(NormalDungeon.Instance.dungeonId);
        m_DungeonInfo.Display(NormalDungeon.Instance.dungeonId);
        m_SingleChallenge.SetActive(NormalDungeon.Instance.showSingleEnterance.Fetch());
        m_TeamChallenge.SetActive(NormalDungeon.Instance.showMultipEnterance.Fetch());

        m_BossModel.Display(config.boss);
    }

    private void DisplaySweepButton()
    {
        m_Sweep.SetActive(NormalDungeon.Instance.showSweep.Fetch());
    }

    private void DisplayChallengeState()
    {
        var surplusTime = NormalDungeon.Instance.surplurTime.Fetch();
        m_SingleChallenge.SetState(surplusTime > 0 ? ButtonEx.State.Normal : ButtonEx.State.Disable);
        m_TeamChallenge.SetState(surplusTime > 0 ? ButtonEx.State.Normal : ButtonEx.State.Disable);
        m_Sweep.SetState(surplusTime > 0 ? ButtonEx.State.Normal : ButtonEx.State.Disable);
    }

}





