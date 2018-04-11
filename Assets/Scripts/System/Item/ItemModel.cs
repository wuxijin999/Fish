﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : ModelBase<ItemModel>
{
    Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    public Item GetItemByInstanceId(int _instanceId)
    {
        Item item;
        itemDictionary.TryGetValue(_instanceId, out item);

        return item;
    }

    public List<Item> GetItemsById(int _id)
    {
        var items = new List<Item>();
        foreach (var item in itemDictionary.Values)
        {
            if (item.id == _id)
            {
                items.Add(item);
            }
        }

        return items;
    }

    public int GetItemCount(int _id)
    {
        var sum = 0;
        foreach (var item in itemDictionary.Values)
        {
            if (item.id == _id)
            {
                sum += item.count;
            }
        }

        return sum;
    }


}
