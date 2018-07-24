using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TestBiz
{

    public JobProperty<string> testString1 = new JobProperty<string>();
    public JobProperty<int> testInt1 = new JobProperty<int>();



    public void UpdateString()
    {
        ThreadPool.QueueUserWorkItem(
    (object aaa) =>
    {
        var index = 0;
        while (index < 10)
        {
            testString1.value += "aaa ";
            testString1.dirty = true;
            index++;
            Thread.Sleep(1000);
        }
    }
    );
    }

    public void UpdateInt()
    {
        ThreadPool.QueueUserWorkItem(
            (object aaa) =>
            {
                var index = 0;
                while (index < 10)
                {
                    testInt1.value += 10;
                    testInt1.dirty = true;
                    index++;
                    Thread.Sleep(1000);
                }
            }
            );
    }


}
