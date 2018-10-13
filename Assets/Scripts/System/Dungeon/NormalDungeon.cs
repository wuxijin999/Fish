//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDungeon : Presenter<NormalDungeon>, IPresenterWindow, IPresenterInit, IPresenterUnInit
{

    public int dungeonId { get; private set; }

    public readonly IntProperty bossId = new IntProperty();
    public readonly IntProperty surplurTime = new IntProperty();
    public readonly BoolProperty showSweep = new BoolProperty();
    public readonly BoolProperty showSingleEnterance = new BoolProperty();
    public readonly BoolProperty showMultipEnterance = new BoolProperty();

    public void Init()
    {
        Dungeon.Instance.dungeonInfoUpdateEvent += this.OnDungeonInfoUpdate;
    }

    public void UnInit()
    {
        Dungeon.Instance.dungeonInfoUpdateEvent -= this.OnDungeonInfoUpdate;
    }

    public void OpenWindow(object @object)
    {
        this.dungeonId = (int)@object;
        var config = DungeonConfig.Get(this.dungeonId);

        bossId.value = config.boss;
        surplurTime.value = Dungeon.Instance.GetDailySurplusTimes(dungeonId);
        showSweep.value = Dungeon.Instance.GetDungeonHighestStar(dungeonId) > 0;
        showSingleEnterance.value = config.type == 1 || config.type == 3;
        showMultipEnterance.value = config.type == 2 || config.type == 3;
        Windows.Instance.Open(WindowType.NormalDungeon);
    }

    public void CloseWindow()
    {
        Windows.Instance.Close(WindowType.NormalDungeon);
    }

    public void EnterDungeon()
    {
        Dungeon.Instance.EneterDungeon(dungeonId);
    }

    public void EnterDungeonGroup()
    {
        Dungeon.Instance.EneterDungeonGroup(dungeonId);
    }

    public void Sweep()
    {

    }

    private void OnDungeonInfoUpdate(int dungeonId)
    {
        if (this.dungeonId != dungeonId)
        {
            return;
        }

        surplurTime.value = Dungeon.Instance.GetDailySurplusTimes(dungeonId);
        showSweep.value = Dungeon.Instance.GetDungeonHighestStar(dungeonId) > 0;
    }


}





