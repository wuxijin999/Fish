//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 17, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWin : Window
{

    [SerializeField] ItemBehaviour m_ItemBehaviour;
    [SerializeField] TextEx m_Type;
    [SerializeField] TextEx m_ItemName;
    [SerializeField] TextEx m_Description;

    #region Built-in

    protected override void SetListeners()
    {
    }

    protected override void OnPreOpen()
    {
        DisplayBaseInfo();
    }

    protected override void OnPreClose()
    {
    }

    #endregion

    private void DisplayBaseInfo()
    {
        this.m_ItemBehaviour.SetItem(ItemInfo.Instance.showItem);
    }

}





