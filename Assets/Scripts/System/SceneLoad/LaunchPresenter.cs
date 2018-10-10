//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPresenter : Presenter<LaunchPresenter>
{

    public readonly FloatProperty progress = new FloatProperty();
    public readonly IntProperty randowTips = new IntProperty();

    public override void OpenWindow(int functionId = 0)
    {
        randowTips.value = UnityEngine.Random.Range(1, 9);
        Windows.Instance.Open(WindowType.Launch);
    }

    public override void CloseWindow()
    {
        Windows.Instance.Close(WindowType.Launch);
    }



}





