﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPrecious : Presenter<FindPrecious>
{

    public readonly IntProperty functionId = new IntProperty(0);

    public override void CloseWindow()
    {
    }

    public override void OpenWindow(int functionId)
    {
        this.functionId.value = functionId;
    }


    

}





