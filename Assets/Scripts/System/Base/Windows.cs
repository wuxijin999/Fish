using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : SingletonMonobehaviour<Window>
{

    Dictionary<WindowType, Window> windows = new Dictionary<WindowType, Window>();
    List<WindowType> openTasks = new List<WindowType>();
    OrderAdminister orderAdminister = new OrderAdminister();

    public void Open(WindowType _type)
    {
        if (!openTasks.Contains(_type))
        {
            openTasks.Add(_type);
        }
    }

    public void Close(WindowType _type)
    {
        if (openTasks.Contains(_type))
        {
            openTasks.Remove(_type);
        }
    }

    public void CloseAll()
    {
        foreach (var window in windows.Values)
        {
            if (window != null && window.windowState == Window.WindowState.Opened)
            {
                window.Close(true);
            }
        }
    }

    public void CloseOthers(WindowType _type)
    {
        foreach (var keyValue in windows)
        {
            if (keyValue.Key == _type)
            {
                continue;
            }

            if (keyValue.Value != null && keyValue.Value.windowState == Window.WindowState.Opened)
            {
                keyValue.Value.Close(true);
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < openTasks.Count; i++)
        {
            var task = openTasks[i];
            GetInstance(task);
            var window = windows[task];
            if (window != null)
            {
                var order = orderAdminister.ApplyFor();
                window.Open(order);
            }
        }
    }

    private void GetInstance(WindowType _type)
    {
        if (!windows.ContainsKey(_type))
        {
            var prefab = UILoader.LoadWindow(_type.ToString());
            var instance = GameObject.Instantiate(prefab);
            var window = instance.GetComponent<Window>();
            if (window != null)
            {
                windows[_type] = window;
            }
        }
    }


    class OrderAdminister
    {
        const int dynamicMin = 1000;
        const int dynamicMax = 5000;
        const int step = 10;

        public int hightestOrder = 1000;

        public int ApplyFor()
        {
            return hightestOrder = hightestOrder + 10;
        }

        public void ResetHightestOrher(int _order)
        {
            hightestOrder = _order;
        }

    }


}

public enum WindowType
{
    Main,
    Role,
    Backpack,
    Setting,
}
