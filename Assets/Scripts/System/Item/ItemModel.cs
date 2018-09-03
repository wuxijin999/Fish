using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : Model<ItemModel>
{
    Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    public override void Init()
    {
    }

    public override void UnInit()
    {
    }

    public override void OnSwitchAccount()
    {
    }

    public override void OnLoginOk()
    {
    }

    public Item GetItemByInstanceId(int instanceId)
    {
        Item item;
        itemDictionary.TryGetValue(instanceId, out item);

        return item;
    }

    public List<Item> GetItemsById(int id)
    {
        var items = new List<Item>();
        foreach (var item in itemDictionary.Values)
        {
            if (item.id == id)
            {
                items.Add(item);
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


}
