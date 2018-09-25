using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock
{
    System.Action onAlarm;

    public readonly ClockType type;
    public readonly float endTime2;
    public readonly DateTime endTime1;

    bool repeat = false;
    float interval = 10;
    float nextAlarmTime2 = 0f;
    DateTime nextAlarmTime1 = DateTime.MinValue;

    public bool end { get; private set; }

    public Clock(ClockParams value, System.Action _callBack)
    {
        this.type = value.type;
        switch (this.type)
        {
            case ClockType.UnityTimeClock:
                this.endTime2 = Time.time + value.surplusSecond;
                break;
            case ClockType.UnityUnScaleClock:
                this.endTime2 = Time.realtimeSinceStartup + value.surplusSecond;
                break;
            case ClockType.DateTimeClock:
                this.endTime1 = DateTime.Now + new TimeSpan((int)(TimeSpan.TicksPerSecond * value.surplusSecond));
                break;
            default:
                break;
        }

        this.repeat = value.repeat;
        this.interval = value.interval;

        if (repeat)
        {
            switch (this.type)
            {
                case ClockType.UnityTimeClock:
                    this.nextAlarmTime2 = Time.time + value.interval;
                    break;
                case ClockType.UnityUnScaleClock:
                    this.nextAlarmTime2 = Time.realtimeSinceStartup + value.interval;
                    break;
                case ClockType.DateTimeClock:
                    this.endTime1 = DateTime.Now + new TimeSpan((int)(TimeSpan.TicksPerSecond * value.interval));
                    break;
                default:
                    break;
            }
        }

        this.onAlarm = _callBack;
    }

    public void Invoke()
    {
        switch (type)
        {
            case ClockType.DateTimeClock:
                if (this.repeat)
                {
                    if (DateTime.Now > this.nextAlarmTime1)
                    {
                        Alarm();
                        this.nextAlarmTime1 += new TimeSpan((int)(TimeSpan.TicksPerSecond * this.interval));
                    }
                }
                else
                {
                    if (DateTime.Now > endTime1)
                    {
                        Alarm();
                        end = true;
                    }
                }
                break;
            case ClockType.UnityTimeClock:
                if (this.repeat)
                {
                    if (Time.time > this.nextAlarmTime2)
                    {
                        Alarm();
                        this.nextAlarmTime2 += this.interval;
                    }
                }
                else
                {
                    if (Time.time > this.endTime2)
                    {
                        Alarm();
                        end = true;
                    }
                }
                break;
            case ClockType.UnityUnScaleClock:
                if (this.repeat)
                {
                    if (Time.realtimeSinceStartup > this.nextAlarmTime2)
                    {
                        Alarm();
                        this.nextAlarmTime2 += this.interval;
                    }
                }
                else
                {
                    if (Time.realtimeSinceStartup > this.endTime2)
                    {
                        Alarm();
                        end = true;
                    }
                }
                break;
        }
    }

    public void Stop()
    {
        this.onAlarm = null;
        end = true;
    }

    void Alarm()
    {
        if (this.onAlarm != null)
        {
            this.onAlarm();
            this.onAlarm = null;
        }
    }

    public enum ClockType
    {
        DateTimeClock,
        UnityTimeClock,
        UnityUnScaleClock,
    }

    public struct ClockParams
    {
        public ClockType type;
        public bool repeat;
        public float interval;
        public float surplusSecond;
    }
}

public class ClockUtil : SingletonMonobehaviour<ClockUtil>
{
    List<Clock> clocks = new List<Clock>();

    public Clock Create(Clock.ClockParams value, System.Action callBack)
    {
        var clock = new Clock(value, callBack);
        clocks.Add(clock);
        return clock;
    }

    public void Clear()
    {
        clocks.Clear();
    }

    public void Dispose()
    {
        foreach (var item in clocks)
        {
            item.Stop();
        }

        clocks.Clear();
    }

    private void Update()
    {
        for (int i = clocks.Count - 1; i >= 0; i--)
        {
            var clock = clocks[i];
            clock.Invoke();
            if (clock.end)
            {
                clocks.RemoveAt(i);
            }
        }
    }

}
