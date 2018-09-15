using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTest : MonoBehaviour
{


    public int second = 9999;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < 100; i++)
            {
                second.ToTimeString("dd:HH:mm:ss");
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < 100; i++)
            {
                System.DateTime.Now.ToShortDateString();
            }
        }
    }
}
