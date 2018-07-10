using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock
{
    Action onAlarm;
    public readonly DateTime endTime;

    public Clock(DateTime _endTime, Action _callBack)
    {
        endTime = _endTime;
        onAlarm = _callBack;
    }

    public void Alarm()
    {
        if (onAlarm != null)
        {
            onAlarm();
            onAlarm = null;
        }
    }

    static bool inited = false;
    static List<Clock> clocks = new List<Clock>();

    public static void Create(DateTime _endTime, Action _callBack)
    {
        if (!inited)
        {
            GlobalTimeEvent.Instance.secondEvent += OnPerSecond;
            inited = true;
        }

        clocks.Add(new Clock(_endTime, _callBack));
    }

    public static void Clear()
    {
        clocks.Clear();
    }

    public static void Dispose()
    {
        clocks.Clear();
        inited = false;
        GlobalTimeEvent.Instance.secondEvent -= OnPerSecond;
    }

    private static void OnPerSecond()
    {
        for (int i = clocks.Count - 1; i >= 0; i--)
        {
            var clock = clocks[i];
            if (DateTime.Now > clock.endTime)
            {
                clock.Alarm();
                clocks.Remove(clock);
            }
        }
    }


}
