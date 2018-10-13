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

    public class Grid
    {
        public readonly int index;
        public readonly StringProperty guid = new StringProperty();
    }

}





