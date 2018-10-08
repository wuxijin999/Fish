using System.Collections.Generic;

public class RedpointCenter : Singleton<RedpointCenter>
{

    Dictionary<int, Redpoint> redpoints = new Dictionary<int, Redpoint>();
    Dictionary<int, List<int>> parentChildren = new Dictionary<int, List<int>>();

    public void Register(int id)
    {
        if (redpoints.ContainsKey(id))
        {
            return;
        }

        var redpoint = new Redpoint(id);
        redpoints[id] = redpoint;
    }

    public void Register(int parent, int id)
    {
        if (redpoints.ContainsKey(id))
        {
            return;
        }

        var redpoint = new Redpoint(id);
        redpoints[id] = redpoint;

        if (parent > 0)
        {
            List<int> children = null;
            if (!parentChildren.TryGetValue(parent, out children))
            {
                parentChildren[parent] = children = new List<int>();
            }

            if (!children.Contains(id))
            {
                children.Add(id);
            }
        }
    }

    public void SetRedPointState(int id, RedPointState state)
    {
        if (!redpoints.ContainsKey(id))
        {
            return;
        }

        var redpoint = redpoints[id];
        redpoint.SetState(state);

        var parentId = redpoint.parent;
        if (parentId > 0)
        {
            UpdateParentValue(parentId);
        }
    }

    public void SetRedPointState(int id, RedPointState state, int count)
    {
        if (!redpoints.ContainsKey(id))
        {
            return;
        }

        var redpoint = redpoints[id];
        redpoint.SetState(state, count);

        var parentId = redpoint.parent;
        if (parentId > 0)
        {
            UpdateParentValue(parentId);
        }
    }

    public Redpoint GetRedpoint(int id)
    {
        if (redpoints.ContainsKey(id))
        {
            return redpoints[id];
        }
        else
        {
            return null;
        }
    }

    public void ResetAllRedpointState()
    {
        foreach (var redpoint in redpoints.Values)
        {
            redpoint.SetState(RedPointState.None);
        }
    }

    public RedPointState GetRedpointState(int id)
    {
        Redpoint redpoint = null;
        if (this.redpoints.TryGetValue(id, out redpoint))
        {
            return redpoint.state.value;
        }
        else
        {
            return RedPointState.None;
        }
    }

    public int GetRedpointCount(int id)
    {
        Redpoint redpoint = null;
        if (this.redpoints.TryGetValue(id, out redpoint))
        {
            return redpoint.count.value;
        }
        else
        {
            return 0;
        }
    }

    void UpdateParentValue(int parentId)
    {
        List<int> children = null;
        Redpoint parent = null;
        if (redpoints.TryGetValue(parentId, out parent) && parentChildren.TryGetValue(parentId, out children))
        {
            var parentState = RedPointState.None;
            foreach (var item in children)
            {
                Redpoint child = null;
                if (redpoints.TryGetValue(item, out child))
                {
                    if (child.state.value > parentState)
                    {
                        parentState = child.state.value;
                    }
                }

                if (parentState == RedPointState.Full)
                {
                    break;
                }
            }

            parent.SetState(parentState);
        }
    }

}

