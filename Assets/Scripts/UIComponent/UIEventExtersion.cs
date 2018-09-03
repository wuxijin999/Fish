using System;
using UnityEngine.Events;
using System.Collections.Generic;

[Serializable]
public class UIEvent : UnityEvent
{
    public UIEvent() : base()
    {

    }
}

[Serializable]
public class UIEventBool : UnityEvent<bool>
{
    public UIEventBool() : base()
    {

    }
}

[Serializable]
public class UIEventInt : UnityEvent<int>
{
    public UIEventInt() : base()
    {

    }
}

[Serializable]
public class UIEventFloat : UnityEvent<float>
{
    public UIEventFloat() : base()
    {

    }
}

[Serializable]
public class UIEventString : UnityEvent<string>
{
    public UIEventString() : base()
    {

    }
}


public class BizEvent
{

    event System.Action bizEvent;
    public BizEvent()
    {

    }

    public void Invoke()
    {
        if (bizEvent != null)
        {
            bizEvent();
        }
    }

    public void Dispose()
    {
        bizEvent = null;
    }

    public static BizEvent operator +(BizEvent @event, System.Action action)
    {
        if (@event != null)
        {
            @event.bizEvent += action;
        }
        return @event;
    }

    public static BizEvent operator -(BizEvent @event, System.Action action)
    {
        if (@event != null)
        {
            @event.bizEvent -= action;
        }
        return @event;
    }

}

public class BizEvent<T>
{

    event Action<T> bizEvent;
    public BizEvent()
    {

    }

    public void Invoke(T value)
    {
        if (bizEvent != null)
        {
            bizEvent(value);
        }
    }

    public void Dispose()
    {
        bizEvent = null;
    }

    public static BizEvent<T> operator +(BizEvent<T> @event, Action<T> action)
    {
        if (@event != null)
        {
            @event.bizEvent += action;
        }
        return @event;
    }

    public static BizEvent<T> operator -(BizEvent<T> @event, Action<T> action)
    {
        if (@event != null)
        {
            @event.bizEvent -= action;
        }
        return @event;
    }


}

public class BizEvent<T0, T1>
{

    event Action<T0, T1> bizEvent;
    public BizEvent()
    {

    }

    public void Invoke(T0 value0, T1 value1)
    {
        if (bizEvent != null)
        {
            bizEvent(value0, value1);
        }
    }

    public void Dispose()
    {
        bizEvent = null;
    }

    public static BizEvent<T0, T1> operator +(BizEvent<T0, T1> @event, Action<T0, T1> action)
    {
        if (@event != null)
        {
            @event.bizEvent += action;
        }
        return @event;
    }

    public static BizEvent<T0, T1> operator -(BizEvent<T0, T1> @event, Action<T0, T1> action)
    {
        if (@event != null)
        {
            @event.bizEvent -= action;
        }
        return @event;
    }

}

public class BizEvent<T0, T1, T2>
{

    event Action<T0, T1, T2> bizEvent;
    public BizEvent()
    {

    }

    public void Invoke(T0 value0, T1 value1, T2 value2)
    {
        if (bizEvent != null)
        {
            bizEvent(value0, value1, value2);
        }
    }

    public void Dispose()
    {
        bizEvent = null;
    }

    public static BizEvent<T0, T1, T2> operator +(BizEvent<T0, T1, T2> @event, Action<T0, T1, T2> action)
    {
        if (@event != null)
        {
            @event.bizEvent += action;
        }
        return @event;
    }

    public static BizEvent<T0, T1, T2> operator -(BizEvent<T0, T1, T2> @event, Action<T0, T1, T2> action)
    {
        if (@event != null)
        {
            @event.bizEvent -= action;
        }
        return @event;
    }


}