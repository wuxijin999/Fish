//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, November 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWinPresenter : Presenter<MainWinPresenter>, IPresenterWindow
{

    public void CloseWindow()
    {
    }

    public void OpenWindow(object @object = null)
    {
        Windows.Instance.Open(WindowType.Main);
    }
}





