using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : Model
{
    Dictionary<string, ItemEntry> itemDictionary = new Dictionary<string, ItemEntry>();

    public override void Reset()
    {
    }

    public void UpdateItem(string guid, int id, int count)
    {
        if (count > 0)
        {
            itemDictionary[guid] = new ItemEntry()
            {
                guid = guid,
                id = id,
                count = count,
            };
        }
        else
        {
            itemDictionary.Remove(guid);
        }
    }

    public bool TryGetItemByGuid(string guid, out ItemEntry item)
    {
        return this.itemDictionary.TryGetValue(guid, out item);
    }

    public List<string> GetItemsById(int id)
    {
        var items = new List<string>();
        foreach (var item in this.itemDictionary.Values)
        {
            if (item.id == id)
            {
                items.Add(item.guid);
            }
        }

        return items;
    }

    public int GetItemCount(int id)
    {
        var sum = 0;
        foreach (var item in this.itemDictionary.Values)
        {
            if (item.id == id)
            {
                sum += item.count;
            }
        }

        return sum;
    }

    public List<ItemEntry> GetItems()
    {
        return new List<ItemEntry>(itemDictionary.Values);
    }

}

public struct ItemEntry
{
    public string guid;
    public int id;
    public int count;

    public ItemEntry SetCount(int count)
    {
        this.count = count;
        return this;
    }

}