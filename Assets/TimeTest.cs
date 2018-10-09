using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class TimeTest : MonoBehaviour
{

    public string input = @"this is a <Item=10007>||||<Npc=1>|<Npc=2>";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(RichTextUtil.Convert(input));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            MultipleTask();
        }
    }

    void MultipleTask()
    {

        ThreadPool.QueueUserWorkItem(
            (object a) =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Debug.Log(RichTextUtil.Convert(input));
                    Thread.Sleep(30);
                }
            }
            );

    }

}
