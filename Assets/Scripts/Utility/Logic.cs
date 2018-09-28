
public class BizProperty<T>
{
    T m_Value;
    public T value
    {
        get { return m_Value; }
        set
        {
            lock (this)
            {
                m_Value = value;
                dirty = true;
            }
        }
    }

    bool m_Dirty = true;
    public bool dirty
    {
        get
        {
            return this.m_Dirty;
        }
        set
        {
            lock (this)
            {
                this.m_Dirty = value;
            }
        }
    }

    public BizProperty()
    {

    }

    public BizProperty(T value)
    {
        this.value = value;
    }

    public T Peek()
    {
        return m_Value;
    }

    public T Fetch()
    {
        this.dirty = false;
        return m_Value;
    }

}
