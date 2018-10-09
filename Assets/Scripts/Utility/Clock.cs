using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock
{
    System.Action onAlarm;

    public readonly ClockType type;
    public readonly float endSecond;
    public readonly DateTime endDateTime;

    bool repeat = false;
    float interval = 10;
    float nextAlarmSecond = 0f;
    DateTime nextAlarmDateTime = DateTime.MinValue;

    public bool end { get; private set; }

    public Clock(ClockParams value, System.Action _callBack)
    {
        this.type = value.type;
        switch (this.type)
        {
            case ClockType.UnityTimeClock:
                this.endSecond = Time.time + value.second;
                break;
            case ClockType.UnityRealTimeClock:
                this.endSecond = Time.realtimeSinceStartup + value.second;
                break;
            case ClockType.DateTimeClock:
                this.endDateTime = DateTime.Now + new TimeSpan((int)(TimeSpan.TicksPerSecond * value.second));
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
                    this.nextAlarmSecond = Time.time + value.interval;
                    break;
                case ClockType.UnityRealTimeClock:
                    this.nextAlarmSecond = Time.realtimeSinceStartup + value.interval;
                    break;
                case ClockType.DateTimeClock:
                    this.endDateTime = DateTime.Now + new TimeSpan((int)(TimeSpan.TicksPerSecond * value.interval));
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
                    if (DateTime.Now > this.nextAlarmDateTime)
                    {
                        Alarm();
                        this.nextAlarmDateTime += new TimeSpan((int)(TimeSpan.TicksPerSecond * this.interval));
                    }
                }
                else
                {
                    if (DateTime.Now > endDateTime)
                    {
                        Alarm();
                        end = true;
                    }
                }
                break;
            case ClockType.UnityTimeClock:
                if (this.repeat)
                {
                    if (Time.time > this.nextAlarmSecond)
                    {
                        Alarm();
                        this.nextAlarmSecond += this.interval;
                    }
                }
                else
                {
                    if (Time.time > this.endSecond)
                    {
                        Alarm();
                        end = true;
                    }
                }
                break;
            case ClockType.UnityRealTimeClock:
                if (this.repeat)
                {
                    if (Time.realtimeSinceStartup > this.nextAlarmSecond)
                    {
                        Alarm();
                        this.nextAlarmSecond += this.interval;
                    }
                }
                else
                {
                    if (Time.realtimeSinceStartup > this.endSecond)
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
        UnityRealTimeClock,
    }

    public struct ClockParams
    {
        public ClockType type;
        public bool repeat;
        public float interval;
        public float second;
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
