using System;

public class BaseProperty
{
    bool m_Dirty = true;
    public bool dirty {
        get {
            return this.m_Dirty;
        }
        set {
            lock (this)
            {
                this.m_Dirty = value;
            }
        }
    }

    public BaseProperty()
    {

    }
}

public sealed class IntProperty : BaseProperty
{
    int m_Value;
    public int value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public IntProperty()
    {
    }

    public IntProperty(int value)
    {
        this.value = value;
    }

    public int Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}

public sealed class FloatProperty : BaseProperty
{
    float m_Value;
    public float value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public FloatProperty()
    {
    }

    public FloatProperty(float value)
    {
        this.value = value;
    }

    public float Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}

public sealed class BoolProperty : BaseProperty
{
    bool m_Value;
    public bool value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public BoolProperty()
    {
    }

    public BoolProperty(bool value)
    {
        this.value = value;
    }

    public bool Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}


public sealed class StringProperty : BaseProperty
{
    string m_Value = string.Empty;
    public string value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public StringProperty()
    {
    }

    public StringProperty(string value)
    {
        this.value = value;
    }

    public string Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}


public sealed class LongProperty : BaseProperty
{
    long m_Value;
    public long value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public LongProperty()
    {
    }

    public LongProperty(long value)
    {
        this.value = value;
    }

    public long Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}

public sealed class Int2Property : BaseProperty
{
    Int2 m_Value = Int2.zero;
    public Int2 value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public Int2Property()
    {
    }

    public Int2Property(Int2 value)
    {
        this.value = value;
    }

    public Int2 Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}

public sealed class Int3Property : BaseProperty
{
    Int3 m_Value = Int3.zero;
    public Int3 value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public Int3Property()
    {
    }

    public Int3Property(Int3 value)
    {
        this.value = value;
    }

    public Int3 Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}

public sealed class DateTimeProperty : BaseProperty
{
    DateTime m_Value = DateTime.MinValue;
    public DateTime value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                if (this.m_Value != value)
                {
                    this.m_Value = value;
                    this.dirty = true;
                }
            }
        }
    }

    public DateTimeProperty()
    {
    }

    public DateTimeProperty(DateTime value)
    {
        this.value = value;
    }

    public DateTime Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}

public sealed class EnumProperty<T> : BaseProperty where T : struct
{
    T m_Value;
    public T value {
        get { return this.m_Value; }
        set {
            lock (this)
            {
                this.m_Value = value;
                this.dirty = true;
            }
        }
    }

    public EnumProperty(T value)
    {
        if (typeof(Enum) != typeof(T).BaseType)
        {
            throw new ArgumentException("参数必须是枚举类型！");
        }

        this.value = value;
    }

    public T Fetch()
    {
        this.dirty = false;
        return this.m_Value;
    }

}
