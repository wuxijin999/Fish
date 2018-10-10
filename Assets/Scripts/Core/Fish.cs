//----------------------------------------------------------------------
//   这是一个全局类型，整个项目中具有最高地位
//   没有得到主程许可 , 不允许更改这个类型
//----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

public class Fish
{
    static Dictionary<BroadcastType, BizEvent> events = new Dictionary<BroadcastType, BizEvent>();

    public static void AddLisenter(BroadcastType type, Action action)
    {
        var bizEvent = events.ContainsKey(type) ? events[type] : events[type] = new BizEvent();
        bizEvent += action;
    }

    public static void RemoveLisenter(BroadcastType type, Action action)
    {
        if (events.ContainsKey(type))
        {
            events[type] -= action;
        }
    }

    public static void Broadcast(BroadcastType type)
    {
        if (events.ContainsKey(type))
        {
            try
            {
                events[type].Invoke();
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }

}

public enum BroadcastType
{
    LoginOk,
    SwitchAccount,
}