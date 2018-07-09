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

    public void PushTask(Task _task)
    {
        var exist = false;
        for (int i = taskQueue.Count - 1; i >= 0; i--)
        {
            var task = taskQueue[i];
            if (task.windowName == _task.windowName)
            {
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            taskQueue.Add(_task);
        }
    }

    public void StopTask(string _name)
    {
        for (int i = taskQueue.Count - 1; i >= 0; i--)
        {
            var task = taskQueue[i];
            if (task.windowName == _name)
            {
                taskQueue.Remove(task);
                break;
            }
        }

        if (currentTask != null && currentTask.windowName == _name)
        {
            currentTask.Dispose();
            currentTask = null;
            PleaseWaitPresenter.Instance.Hide(PleaseWaitPresenter.WaitType.WindowLoad);
        }
    }

    public void StopAllTasks()
    {
        currentTask = null;
        taskQueue.Clear();
        PleaseWaitPresenter.Instance.Hide(PleaseWaitPresenter.WaitType.WindowLoad);
    }

    private void LateUpdate()
    {
        if (currentTask == null && taskQueue.Count > 0)
        {
            currentTask = taskQueue[0];
            taskQueue.RemoveAt(0);

            UILoader.LoadWindowAsync(currentTask.windowName, (bool ok, UnityEngine.Object _resource) =>
            {
                try
                {
                    if (currentTask != null)
                    {
                        currentTask.Report(ok, _resource);
                        currentTask = null;
                    }
                }
                catch (Exception ex)
                {
                    DebugEx.Log(ex);
                }
                finally
                {
                    PleaseWaitPresenter.Instance.Hide(PleaseWaitPresenter.WaitType.WindowLoad);
                }
            });
        }

    }

    public class TaskGroup
    {
        List<Task> tasks = new List<Task>();

        public void AddTask(Task _task)
        {
            if (!tasks.Contains(_task))
            {
                tasks.Add(_task);
            }
        }

        public void NotifyTaskState(Task _task)
        {
            bool allReady = true;
            for (int i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].ready)
                {
                    allReady = false;
                    break;
                }
            }

            if (allReady)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    tasks[i].Done();
                }
            }
        }

    }

    public class Task
    {
        public TaskGroup taskGroup { get; private set; }
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

        public void Bind(TaskGroup _task)
        {
            taskGroup = _task;
        }

        public void Report(bool _ok, UnityEngine.Object _object)
        {
            result = _ok;
            asset = _object;

            if (taskGroup != null)
            {
                ready = true;
                taskGroup.NotifyTaskState(this);
            }
            else
            {
                Done();
            }
        }

        public void Dispose()
        {
            callBack = null;
        }

        public void Done()
        {
            if (callBack != null)
            {
                callBack(result, asset);
                callBack = null;
            }
        }


    }

}




