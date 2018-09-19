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

    public bool busy { get { return this.currentTask != null; } }

    public void PushTask(Task task)
    {
        var exist = false;
        for (int i = this.taskQueue.Count - 1; i >= 0; i--)
        {
            var temp = this.taskQueue[i];
            if (task.windowName == temp.windowName)
            {
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            this.taskQueue.Add(task);
        }
    }

    public void StopTask(string name)
    {
        for (int i = this.taskQueue.Count - 1; i >= 0; i--)
        {
            var task = this.taskQueue[i];
            if (task.windowName == name)
            {
                this.taskQueue.Remove(task);
                break;
            }
        }

        if (this.currentTask != null && this.currentTask.windowName == name)
        {
            this.currentTask.Dispose();
            this.currentTask = null;
            PleaseWait.Instance.Hide(PleaseWait.WaitType.WindowLoad);
        }
    }

    public void StopAllTasks()
    {
        if (this.currentTask != null)
        {
            this.currentTask.Dispose();
        }
        this.currentTask = null;
        this.taskQueue.Clear();
        PleaseWait.Instance.Hide(PleaseWait.WaitType.WindowLoad);
    }

    private void LateUpdate()
    {
        if (this.currentTask == null && this.taskQueue.Count > 0)
        {
            this.currentTask = this.taskQueue[0];
            this.taskQueue.RemoveAt(0);

            UIAssets.LoadWindowAsync(this.currentTask.windowName, (bool ok, UnityEngine.Object _resource) =>
            {
                try
                {
                    if (this.currentTask != null)
                    {
                        this.currentTask.Done(ok, _resource);
                        this.currentTask = null;
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
            this.windowName = _windowName;
            this.callBack = _callBack;
        }

        public void Done(bool _ok, UnityEngine.Object _object)
        {
            this.result = _ok;
            this.asset = _object;
            if (this.callBack != null)
            {
                this.callBack(this.result, this.asset);
                this.callBack = null;
            }
        }

        public void Dispose()
        {
            this.callBack = null;
        }


    }

}




