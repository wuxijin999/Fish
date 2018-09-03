//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Tuesday, September 26, 2017
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;

public class WindowAsyncLoad : MonoBehaviour
{
    List<Task> taskQueue = new List<Task>();
    Task currentTask;

    public bool busy { get { return currentTask != null; } }

    public void PushTask(Task task)
    {
        var exist = false;
        for (int i = taskQueue.Count - 1; i >= 0; i--)
        {
            var temp = taskQueue[i];
            if (task.windowName == temp.windowName)
            {
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            taskQueue.Add(task);
        }
    }

    public void StopTask(string name)
    {
        for (int i = taskQueue.Count - 1; i >= 0; i--)
        {
            var task = taskQueue[i];
            if (task.windowName == name)
            {
                taskQueue.Remove(task);
                break;
            }
        }

        if (currentTask != null && currentTask.windowName == name)
        {
            currentTask.Dispose();
            currentTask = null;
            PleaseWait.Instance.Hide(PleaseWait.WaitType.WindowLoad);
        }
    }

    public void StopAllTasks()
    {
        if (currentTask != null)
        {
            currentTask.Dispose();
        }
        currentTask = null;
        taskQueue.Clear();
        PleaseWait.Instance.Hide(PleaseWait.WaitType.WindowLoad);
    }

    private void LateUpdate()
    {
        if (currentTask == null && taskQueue.Count > 0)
        {
            currentTask = taskQueue[0];
            taskQueue.RemoveAt(0);

            UIAssets.LoadWindowAsync(currentTask.windowName, (bool ok, UnityEngine.Object _resource) =>
            {
                try
                {
                    if (currentTask != null)
                    {
                        currentTask.Done(ok, _resource);
                        currentTask = null;
                    }
                }
                catch (Exception ex)
                {
                    DebugEx.Log(ex);
                }
                finally
                {
                    PleaseWait.Instance.Hide(PleaseWait.WaitType.WindowLoad);
                }
            });
        }

    }

    public class Task
    {
        public string windowName { get; private set; }
        public bool ready = false;

        Action<bool, UnityEngine.Object> callBack;
        bool result = false;
        UnityEngine.Object asset;

        public Task(string _windowName, Action<bool, UnityEngine.Object> _callBack)
        {
            windowName = _windowName;
            callBack = _callBack;
        }

        public void Done(bool _ok, UnityEngine.Object _object)
        {
            result = _ok;
            asset = _object;
            if (callBack != null)
            {
                callBack(result, asset);
                callBack = null;
            }
        }

        public void Dispose()
        {
            callBack = null;
        }


    }

}




