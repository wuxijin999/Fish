using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : SingletonMonobehaviour<Windows>
{

    Dictionary<WindowType, Window> windows = new Dictionary<WindowType, Window>();
    List<WindowType> openCmds = new List<WindowType>();
    List<WindowType> closeCmds = new List<WindowType>();
    OrderAdminister orderAdminister = new OrderAdminister();

    public void Open(WindowType type)
    {
        if (!openCmds.Contains(type))
        {
            openCmds.Add(type);
        }

        if (closeCmds.Contains(type))
        {
            closeCmds.Remove(type);
        }
    }

    public void Close(WindowType type)
    {
        if (!closeCmds.Contains(type))
        {
            closeCmds.Add(type);
        }

        if (openCmds.Contains(type))
        {
            openCmds.Remove(type);
        }
    }

    public bool IsOpen(WindowType type)
    {
        if (windows.ContainsKey(type))
        {
            return windows[type].windowState == WindowState.Opened;
        }
        else
        {
            return false;
        }
    }

    public void CloseAll()
    {
        foreach (var window in windows.Values)
        {
            if (window != null && window.windowState == WindowState.Opened)
            {
                window.Close();
            }
        }
    }

    public void CloseOthers(WindowType type)
    {
        foreach (var keyValue in windows)
        {
            if (keyValue.Key == type)
            {
                continue;
            }

            if (keyValue.Value != null && keyValue.Value.windowState == WindowState.Opened)
            {
                keyValue.Value.Close();
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < closeCmds.Count; i++)
        {
            var task = closeCmds[i];
            if (windows.ContainsKey(task))
            {
                var window = windows[task];
                if (window != null)
                {
                    window.Close();
                }
            }
        }

        orderAdminister.ResetHightestOrder(GetHighestOrder());

        for (int i = 0; i < openCmds.Count; i++)
        {
            var task = openCmds[i];
            GetInstance(task);
            var window = windows[task];
            if (window != null)
            {
                var order = orderAdminister.ApplyFor();
                window.Open(order);
            }
        }
    }

    private int GetHighestOrder()
    {
        var highestOrder = 1000;
        foreach (var window in windows.Values)
        {
            if (window.order > highestOrder)
            {
                highestOrder = window.order;
            }
        }

        return highestOrder;
    }

    private void GetInstance(WindowType type)
    {
        if (!windows.ContainsKey(type))
        {
            var prefab = UIAssets.LoadWindow(StringUtil.Contact(type, "Win"));
            var instance = GameObject.Instantiate(prefab);
            var window = instance.GetComponent<Window>();
            if (window != null)
            {
                windows[type] = window;
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

        public void ResetHightestOrder(int order)
        {
            hightestOrder = Mathf.Clamp(order, dynamicMin, dynamicMax);
        }

    }

}

public enum WindowType
{
    Main,
    Role,
    Backpack,
    Setting,
    ConfirmCancel,
    PleaseWait,
    Login,
}
