public class Redpoint
{
    public readonly int id;
    public readonly int parent;

    public readonly IntProperty count = new IntProperty(0);
    public readonly EnumProperty<RedPointState> state = new EnumProperty<RedPointState>(RedPointState.None);

    public Redpoint(int id)
    {
        this.id = id;
    }

    public Redpoint(int parent, int id)
    {
        this.parent = parent;
        this.id = id;
    }

    public void SetState(RedPointState state, int count = 0)
    {
        this.state.value = state;
        this.count.value = count;
    }

    public void SetCount(int count)
    {
        this.count.value = count;
    }
}

public enum RedPointState
{
    None,
    Simple,
    Count,
    Full,
}