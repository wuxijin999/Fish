//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, October 12, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : Presenter<Backpack>
{

    List<Grid> grids = new List<Grid>();

    public override void OpenWindow(int functionId = 0)
    {
    }

    public override void CloseWindow()
    {
    }

    public class Grid
    {
        public readonly int index;
        public readonly StringProperty guid = new StringProperty();
    }

}





