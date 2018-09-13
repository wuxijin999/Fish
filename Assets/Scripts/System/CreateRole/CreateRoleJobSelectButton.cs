//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateRoleJobSelectButton : Button
{

    bool m_Selected = false;
    public bool selected {
        get { return m_Selected; }
        set { m_Selected = value; }
    }

}



