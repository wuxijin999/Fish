using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : SingletonMonobehaviour<Windows>
{

    Dictionary<WindowType, Window> windows = new Dictionary<WindowType, Window>();
    List<WindowType> openCmds = new List<WindowType>();
    List<WindowType> closeCmds = new List<WindowType>();
    OrderAdminister orderAdminister = new OrderAdminister();

    public void Open(WindowType _type)
    {
        if (!openCmds.Contains(_type))
        {
            openCmds.Add(_type);
        }

        if (closeCmds.Contains(_type))
        {
            closeCmds.Remove(_type);
        }
    }

    public void Close(WindowType _type)
    {
        if (!closeCmds.Contains(_type))
        {
            closeCmds.Add(_type);
        }

        if (openCmds.Contains(_type))
        {
            openCmds.Remove(_type);
        }
    }

    public bool IsOpen(WindowType _type)
    {
        if (windows.ContainsKey(_type))
        {
            return windows[_type].windowState == WindowState.Opened;
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

    public void CloseOthers(WindowType _type)
    {
        foreach (var keyValue in windows)
        {
            if (keyValue.Key == _type)
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

    private void GetInstance(WindowType _type)
    {
        if (!windows.ContainsKey(_type))
        {
            var prefab = UIAssets.LoadWindow(_type.ToString());
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

        public void ResetHightestOrder(int _order)
        {
            hightestOrder = Mathf.Clamp(_order, dynamicMin, dynamicMax);
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
}
