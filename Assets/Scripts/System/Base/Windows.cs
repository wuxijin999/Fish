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
        if (!this.openCmds.Contains(type))
        {
            this.openCmds.Add(type);
        }

        if (this.closeCmds.Contains(type))
        {
            this.closeCmds.Remove(type);
        }
    }

    public void Close(WindowType type)
    {
        if (!this.closeCmds.Contains(type))
        {
            this.closeCmds.Add(type);
        }

        if (this.openCmds.Contains(type))
        {
            this.openCmds.Remove(type);
        }
    }

    public bool IsOpen(WindowType type)
    {
        if (this.windows.ContainsKey(type))
        {
            return this.windows[type].windowState == WindowState.Opened;
        }
        else
        {
            return false;
        }
    }

    public void CloseAll()
    {
        foreach (var window in this.windows.Values)
        {
            if (window != null && window.windowState == WindowState.Opened)
            {
                window.Close();
            }
        }
    }

    public void CloseOthers(WindowType type)
    {
        foreach (var keyValue in this.windows)
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
        for (int i = 0; i < this.closeCmds.Count; i++)
        {
            var task = this.closeCmds[i];
            if (this.windows.ContainsKey(task))
            {
                var window = this.windows[task];
                if (window != null)
                {
                    window.Close();
                }
            }
        }

        this.orderAdminister.ResetHightestOrder(GetHighestOrder());

        for (int i = 0; i < this.openCmds.Count; i++)
        {
            var task = this.openCmds[i];
            GetInstance(task);
            var window = this.windows[task];
            if (window != null)
            {
                var order = this.orderAdminister.ApplyFor();
                window.Open(order);
            }
        }
    }

    private int GetHighestOrder()
    {
        var highestOrder = 1000;
        foreach (var window in this.windows.Values)
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
        if (!this.windows.ContainsKey(type))
        {
            var prefab = UIAssets.LoadWindow(StringUtil.Contact(type, "Win"));
            var instance = GameObject.Instantiate(prefab);
            var window = instance.GetComponent<Window>();
            if (window != null)
            {
                this.windows[type] = window;
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
            return this.hightestOrder = this.hightestOrder + step;
        }

        public void ResetHightestOrder(int order)
        {
            this.hightestOrder = Mathf.Clamp(order, dynamicMin, dynamicMax);
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
    CreateRole,
    Launch,
    NormalDungeon
}
