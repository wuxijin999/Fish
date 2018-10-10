using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNet : SingletonMonobehaviour<GameNet>
{

    private void Update()
    {

    }

    public enum NetState
    {
        NerverConnected,
        AccountLogin,
        EneterWorld,
    }


}
