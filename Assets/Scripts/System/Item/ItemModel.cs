using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{
    Dictionary<int, ItemEntry> itemDictionary = new Dictionary<int, ItemEntry>();

    public ItemEntry GetItemByInstanceId(int guid)
    {
        ItemEntry item;
        itemDictionary.TryGetValue(guid, out item);

        return item;
    }

    public List<int> GetItemsById(int id)
    {
        var items = new List<int>();
        foreach (var item in itemDictionary.Values)
        {
            if (item.id == id)
            {
                items.Add(item.instanceId);
            }
        }

        return items;
    }

    public int GetItemCount(int id)
    {
        var sum = 0;
        foreach (var item in itemDictionary.Values)
        {
            if (item.id == id)
            {
                sum += item.count;
            }
        }

        return sum;
    }


    public class ItemEntry
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

        public ItemEntry(int _instanceId)
        {
            this.instanceId = _instanceId;
        }

    }

}
