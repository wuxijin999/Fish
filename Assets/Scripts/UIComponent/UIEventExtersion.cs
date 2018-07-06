using System;
using UnityEngine.Events;
using System.Collections.Generic;

[Serializable]
public class UIEvent:UnityEvent {
    public UIEvent() : base() {

    }
}

[Serializable]
public class UIEventBool:UnityEvent<bool> {
    public UIEventBool() : base() {

    }
}

[Serializable]
public class UIEventInt:UnityEvent<int> {
    public UIEventInt() : base() {

    }
}

[Serializable]
public class UIEventFloat:UnityEvent<float> {
    public UIEventFloat() : base() {

    }
}

[Serializable]
public class UIEventString:UnityEvent<string> {
    public UIEventString() : base() {

    }
}


public class BizEvent {

    event Action bizEvent;
    public BizEvent() {

    }

    public void Invoke() {
        if(bizEvent != null) {
            bizEvent();
        }
    }

    public void Dispose() {
        bizEvent = null;
    }

    public static BizEvent operator +(BizEvent _event,Action _action) {
        if(_event != null) {
            _event.bizEvent += _action;
        }
        return _event;
    }

    public static BizEvent operator -(BizEvent _event,Action _action) {
        if(_event != null) {
            _event.bizEvent -= _action;
        }
        return _event;
    }


}

public class BizEvent<T> {

    event Action<T> bizEvent;
    public BizEvent() {

    }

    public void Invoke(T _value) {
        if(bizEvent != null) {
            bizEvent(_value);
        }
    }

    public void Dispose() {
        bizEvent = null;
    }

    public static BizEvent<T> operator +(BizEvent<T> _event,Action<T> _action) {
        if(_event != null) {
            _event.bizEvent += _action;
        }
        return _event;
    }

    public static BizEvent<T> operator -(BizEvent<T> _event,Action<T> _action) {
        if(_event != null) {
            _event.bizEvent -= _action;
        }
        return _event;
    }


}

public class BizEvent<T0, T1> {

    event Action<T0,T1> bizEvent;
    public BizEvent() {

    }

    public void Invoke(T0 _value0,T1 _value1) {
        if(bizEvent != null) {
            bizEvent(_value0,_value1);
        }
    }

    public void Dispose() {
        bizEvent = null;
    }

    public static BizEvent<T0,T1> operator +(BizEvent<T0,T1> _event,Action<T0,T1> _action) {
        if(_event != null) {
            _event.bizEvent += _action;
        }
        return _event;
    }

    public static BizEvent<T0,T1> operator -(BizEvent<T0,T1> _event,Action<T0,T1> _action) {
        if(_event != null) {
            _event.bizEvent -= _action;
        }
        return _event;
    }

}

public class BizEvent<T0, T1, T2> {

    event Action<T0,T1,T2> bizEvent;
    public BizEvent() {

    }

    public void Invoke(T0 _value0,T1 _value1,T2 _value2) {
        if(bizEvent != null) {
            bizEvent(_value0,_value1,_value2);
        }
    }

    public void Dispose() {
        bizEvent = null;
    }

    public static BizEvent<T0,T1,T2> operator +(BizEvent<T0,T1,T2> _event,Action<T0,T1,T2> _action) {
        if(_event != null) {
            _event.bizEvent += _action;
        }
        return _event;
    }

    public static BizEvent<T0,T1,T2> operator -(BizEvent<T0,T1,T2> _event,Action<T0,T1,T2> _action) {
        if(_event != null) {
            _event.bizEvent -= _action;
        }
        return _event;
    }


}