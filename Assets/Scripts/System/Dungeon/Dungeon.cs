//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Presenter<Dungeon>
{
    DungeonModel model = new DungeonModel();

    public void EneterDungeon(int dungeonId)
    {
        var config = DungeonConfig.Get(dungeonId);
    }

}





