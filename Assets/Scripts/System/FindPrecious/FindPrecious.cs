//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPrecious : Presenter<FindPrecious>, IPresenterWindow
{

    public void OpenWindow(object @object)
    {
        this.functionId.value = (int)@object;
    }

    public void CloseWindow()
    {
    }

}





