using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalTimeEvent : SingletonMonobehaviour<GlobalTimeEvent>
{
    public event Action secondEvent;
    public event Action minuteEvent;
    public event Action fiveMinuteEvent;
    public event Action tenMinuteEvent;
    public event Action halfHourEvent;
    public event Action hourEvent;

    int secondBuf = -1;
    int minuteBuf = -1;
    int fiveMinuteBuf = -1;
    int tenMinuteBuf = -1;
    int halfHourBuf = -1;
    int hourBuf = -1;

    public void Begin()
    {

    }

    private void Update()
    {
        var second = DateTime.Now.Second;
        if (second != secondBuf)
        {
            if (secondEvent != null)
            {
                secondEvent();
            }

            secondBuf = second;
        }

        var minute = DateTime.Now.Minute;
        if (minuteBuf != minute)
        {
            try
            {
                if (minuteEvent != null)
                {
                    minuteEvent();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                minuteBuf = minute;
            }
        }

        var fiveMinute = minute / 5;
        if (fiveMinuteBuf != fiveMinute)
        {
            try
            {
                if (fiveMinuteEvent != null)
                {
                    fiveMinuteEvent();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                fiveMinuteBuf = fiveMinute;
            }
        }

        var tenMinute = minute / 10;
        if (tenMinuteBuf != tenMinute)
        {
            try
            {
                if (tenMinuteEvent != null)
                {
                    tenMinuteEvent();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                tenMinuteBuf = tenMinute;
            }
        }

        var thirtyMinute = minute / 30;
        if (halfHourBuf != thirtyMinute)
        {
            try
            {
                if (halfHourEvent != null)
                {
                    halfHourEvent();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                halfHourBuf = thirtyMinute;
            }
        }

        var hour = DateTime.Now.Hour;
        if (hourBuf != hour)
        {
            try
            {
                if (hourEvent != null)
                {
                    hourEvent();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                hourBuf = hour;
            }
        }

    }


}
