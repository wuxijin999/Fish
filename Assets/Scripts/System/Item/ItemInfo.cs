//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, September 15, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : Presenter<ItemInfo>
{

    public Item showItem { get; private set; }

    public override void OpenWindow()
    {
    }

    public override void CloseWindow()
    {
    }

    public void ViewItemInfo(Item item)
    {
        this.showItem = item;
        var config = ItemConfig.Get(this.showItem.id);
        switch (config.type)
        {
            case 1:
                break;
            default:
                break;
        }
    }

}





