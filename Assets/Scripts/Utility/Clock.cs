using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock
{
    System.Action onAlarm;
    public readonly DateTime endTime1;
    public readonly float endTime2;
    public readonly ClockType type;

    public Clock(DateTime _endTime, System.Action _callBack)
    {
        type = ClockType.DateTimeClock;
        endTime1 = _endTime;
        onAlarm = _callBack;
    }

    public Clock(float _endTime, System.Action _callBack)
    {
        type = ClockType.UnityTimeClock;
        endTime2 = _endTime;
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

    public enum ClockType
    {
        DateTimeClock,
        UnityTimeClock,
    }

    static bool inited = false;
    static List<Clock> clocks = new List<Clock>();

    public static void Create(DateTime _endTime, System.Action _callBack)
    {
        if (!inited)
        {
            GlobalTimeEvent.Instance.secondEvent += OnPerSecond;
            inited = true;
        }

        clocks.Add(new Clock(_endTime, _callBack));
    }

    public static void Create(float _endTime, System.Action _callBack)
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
            switch (clock.type)
            {
                case ClockType.DateTimeClock:
                    if (DateTime.Now > clock.endTime1)
                    {
                        clock.Alarm();
                        clocks.Remove(clock);
                    }
                    break;
                case ClockType.UnityTimeClock:
                    if (Time.time > clock.endTime2)
                    {
                        clock.Alarm();
                        clocks.Remove(clock);
                    }
                    break;
            }

        }

    }


}
