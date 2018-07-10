using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tipTest : MonoBehaviour
{

    [SerializeField] PopupTipsWidget m_Widget;

    int index = 100;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            m_Widget.Popup(index.ToString());
            index++;
        }

    }

}
