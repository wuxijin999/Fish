using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    int m_InstanceId;
    public int instanceId
    {
        get { return m_InstanceId; }
        private set { m_InstanceId = value; }
    }

    int m_Id;
    public int id
    {
        get { return m_Id; }
        private set { m_Id = value; }
    }

    int m_Count;
    public int count
    {
        get { return m_Count; }
        set { m_Count = value; }
    }

    public Item(int _instanceId, int _id, int _count)
    {
        this.instanceId = _instanceId;
        this.id = _id;
        this.count = _count;
    }

}
