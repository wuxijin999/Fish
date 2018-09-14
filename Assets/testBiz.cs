using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBiz
{
    public readonly BizProperty<int> testInt = new BizProperty<int>();
    public readonly BizProperty<string> testString = new BizProperty<string>();

    public testBiz()
    {
        testInt.value = 0;
        testString.value = "aaaa";
    }

}

public class BizProperty<T>
{
    public T value;

    bool m_Dirty = true;
    public bool dirty {
        get {
            lock (this)
            {
                return m_Dirty;
            }
        }
        set {
            lock (this)
            {
                m_Dirty = value;
            }
        }
    }

    public BizProperty()
    {

    }

}

public class BizObject
{
    public object value;
    public bool dirty;

    public BizObject()
    {
        value = null;
        dirty = true;
    }

    public BizObject(ref object _value)
    {
        value = _value;
        dirty = true;
    }
}
