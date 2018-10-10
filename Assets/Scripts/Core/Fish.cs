using System;
using System.Collections.Generic;

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
            events[type].Invoke();
        }
    }

}

public enum BroadcastType
{
    LoginOk,
    SwitchAccount,
}