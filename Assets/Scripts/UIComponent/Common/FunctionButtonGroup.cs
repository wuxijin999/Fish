//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Tuesday, October 31, 2017
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunctionButtonGroup : MonoBehaviour
{
    public int unLockedCount {
        get {
            var count = 0;
            foreach (var button in toggleButtons.Values)
            {
                count += button.state != FunctionButton.State.Locked ? 1 : 0;
            }

            return count;
        }
    }

    int currentOrder = 0;
    List<int> orders = new List<int>();
    Dictionary<int, FunctionButton> toggleButtons = new Dictionary<int, FunctionButton>();

    public void Register(FunctionButton _toggleButton)
    {
        toggleButtons[_toggleButton.order] = _toggleButton;
        if (!orders.Contains(_toggleButton.order))
        {
            orders.Add(_toggleButton.order);
            orders.Sort(OrderCompare);
        }
    }

    public void UnRegister(FunctionButton _toggleButton)
    {
        toggleButtons.Remove(_toggleButton.order);

        if (orders.Contains(_toggleButton.order))
        {
            orders.Remove(_toggleButton.order);
            orders.Sort(OrderCompare);
        }
    }

    public void NotifyToggleOn(FunctionButton _toggleButton)
    {
        if (_toggleButton.state == FunctionButton.State.Selected)
        {
            currentOrder = _toggleButton.order;
            for (int i = 0; i < orders.Count; i++)
            {
                var toggleButton = toggleButtons[orders[i]];
                if (toggleButton != _toggleButton && toggleButton.state != FunctionButton.State.Locked)
                {
                    toggleButton.state = FunctionButton.State.Normal;
                }
            }
        }
    }

    public void TriggerByOrder(int _order)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            var order = orders[i];
            if (order == _order)
            {
                toggleButtons[order].Invoke(true);
                break;
            }
        }
    }

    public void TriggerLast()
    {
        var index = orders.IndexOf(currentOrder);
        if (index < 0)
        {
            return;
        }

        var loopTimes = 0;
        while (loopTimes < 2)
        {
            index--;
            if (index < 0)
            {
                index = orders.Count - 1;
                loopTimes++;
            }

            var next = orders[index];
            if (toggleButtons[next].state != FunctionButton.State.Locked)
            {
                toggleButtons[next].Invoke(false);
                break;
            }
        }

    }

    public void TriggerNext()
    {
        var index = orders.IndexOf(currentOrder);
        if (index < 0)
        {
            return;
        }

        var loopTimes = 0;
        while (loopTimes < 2)
        {
            index++;
            if (index > orders.Count - 1)
            {
                index = 0;
                loopTimes++;
            }

            var next = orders[index];
            if (toggleButtons[next].state != FunctionButton.State.Locked)
            {
                toggleButtons[next].Invoke(false);
                break;
            }
        }
    }

    public bool IsFirst()
    {
        return orders.Count > 0 && currentOrder == orders[0];
    }

    public bool IsLast()
    {
        return orders.Count > 0 && currentOrder == orders[orders.Count - 1];
    }

    private int OrderCompare(int a, int b)
    {
        return a < b ? -1 : 1;
    }
}




