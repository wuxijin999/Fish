using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DownLoadTaskUtility
{

    public static long downLoadedSize = 0;

    public static int speed
    {
        get
        {
            return 0;
        }
    }

    static List<DownLoadTask> tasks = new List<DownLoadTask>();

    public static void AddTask(string _url, string _filePath, Action _callBack)
    {
        tasks.Add(new DownLoadTask(_url, _filePath, _callBack));
    }


    public static void StopAllTask()
    {
        foreach (var task in tasks)
        {
            if (task != null)
            {
                task.Stop();
            }
        }

        tasks.Clear();
    }

}
