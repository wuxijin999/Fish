//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, October 12, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPresenter : Presenter<ItemPresenter>
{

    ItemModel model = new ItemModel();

    public ItemEntry GetItemByGuid(string guid)
    {
        ItemEntry itemEntry;
        if (model.TryGetItemByGuid(guid, out itemEntry))
        {
            return itemEntry;
        }
        else
        {
            DebugEx.LogFormat("无法获得Guid为:{0}的物品!", guid);
            return default(ItemEntry);
        }
    }

    public List<string> GetItemsById(int id)
    {
        return model.GetItemsById(id);
    }

    public List<string> GetItemsByType(List<int> types)
    {
        var itemsByType = new List<string>();
        var items = model.GetItems();
        foreach (var item in items)
        {
            var config = ItemConfig.Get(item.id);
            if (types.Contains(config.type))
            {
                itemsByType.Add(item.guid);
            }
        }

        return itemsByType;
    }

    public List<string> GetItemsEqualsOrLargerLevel(int level)
    {
        var itemsByLevel = new List<string>();
        var items = model.GetItems();
        foreach (var item in items)
        {
            var config = ItemConfig.Get(item.id);
            if (config.level == level)
            {
                itemsByLevel.Add(item.guid);
            }
        }

        return itemsByLevel;
    }

    public List<string> GetItemsByQuality(int quality)
    {
        var itemsByQuality = new List<string>();
        var items = model.GetItems();
        foreach (var item in items)
        {
            var config = ItemConfig.Get(item.id);
            if (config.quality == quality)
            {
                itemsByQuality.Add(item.guid);
            }
        }

        return itemsByQuality;
    }

    public List<string> GetItemsEqualsOrLargerQuality(int quality)
    {
        var itemsByQuality = new List<string>();
        var items = model.GetItems();
        foreach (var item in items)
        {
            var config = ItemConfig.Get(item.id);
            if (config.quality >= quality)
            {
                itemsByQuality.Add(item.guid);
            }
        }

        return itemsByQuality;
    }

    public List<string> GetItemsByStarLevel(int starLevel)
    {
        var itemsByStarLevel = new List<string>();
        var items = model.GetItems();
        foreach (var item in items)
        {
            var config = ItemConfig.Get(item.id);
            if (config.starLevel == starLevel)
            {
                itemsByStarLevel.Add(item.guid);
            }
        }

        return itemsByStarLevel;
    }

    public List<string> GetItemsEqualsOrLargerStarLevel(int starLevel)
    {
        var itemsByStarLevel = new List<string>();
        var items = model.GetItems();
        foreach (var item in items)
        {
            var config = ItemConfig.Get(item.id);
            if (config.starLevel >= starLevel)
            {
                itemsByStarLevel.Add(item.guid);
            }
        }

        return itemsByStarLevel;
    }


}





