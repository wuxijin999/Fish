using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalTimeEvent : SingletonMonobehaviour<GlobalTimeEvent>
{
    public event System.Action secondEvent;
    public event System.Action minuteEvent;
    public event System.Action fiveMinuteEvent;
    public event System.Action tenMinuteEvent;
    public event System.Action halfHourEvent;
    public event System.Action hourEvent;

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
        if (second != this.secondBuf)
        {
            if (secondEvent != null)
            {
                secondEvent();
            }

            this.secondBuf = second;
        }

        var minute = DateTime.Now.Minute;
        if (this.minuteBuf != minute)
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
                this.minuteBuf = minute;
            }
        }

        var fiveMinute = minute / 5;
        if (this.fiveMinuteBuf != fiveMinute)
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
                this.fiveMinuteBuf = fiveMinute;
            }
        }

        var tenMinute = minute / 10;
        if (this.tenMinuteBuf != tenMinute)
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
                this.tenMinuteBuf = tenMinute;
            }
        }

        var thirtyMinute = minute / 30;
        if (this.halfHourBuf != thirtyMinute)
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
                this.halfHourBuf = thirtyMinute;
            }
        }

        var hour = DateTime.Now.Hour;
        if (this.hourBuf != hour)
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
                this.hourBuf = hour;
            }
        }

    }


}
