//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGroupBehaviour : UIBase
{

    [SerializeField] ItemBehaviour[] m_ItemBehaviours;

    public void Display(List<Item> items)
    {
        if (items == null)
        {
            return;
        }

        for (var i = 0; i < m_ItemBehaviours.Length; i++)
        {
            var behaviour = m_ItemBehaviours[i];
            if (i < items.Count)
            {
                behaviour.SetItem(items[i]);
                behaviour.SetActive(true);
            }
            else
            {
                behaviour.SetActive(false);
            }
        }
    }


}



