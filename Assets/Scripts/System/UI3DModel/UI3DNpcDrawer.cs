﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;

public class UI3DNpcDrawer : UI3DModelDrawer
{

    int npcId = 0;

    public override void Display(int npcId)
    {
        this.npcId = npcId;
    }

}


