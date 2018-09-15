//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, September 15, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ItemBehaviour : UIBase
{

    [SerializeField] ImageEx m_Icon;
    [SerializeField] ImageEx m_BackGround;
    [SerializeField] RectTransform m_CountContainer;
    [SerializeField] TextEx m_Count;
    [SerializeField] RectTransform[] m_Stars;
    [SerializeField] ButtonEx m_Button;

    UnityAction onClick;

    int instanceId;
    int id;
    int count;

    public void SetItem(Item item)
    {
        this.instanceId = item.instanceId;
        this.id = item.id;
        this.count = item.count;
        DisplayItemBaseInfo();
        DisplayCountInfo();
    }

    public void SetCount(int count)
    {
        this.count = count;
        DisplayCountInfo();
    }

    public void SetListener(UnityAction action)
    {
        onClick = action;
    }

    public void RemoveListener()
    {
        onClick = null;
    }

    private void OnEnable()
    {
        m_Button.SetListener(OnClick);
    }

    private void DisplayItemBaseInfo()
    {
        var config = ItemConfig.Get(id);
        m_Icon.SetSprite(config.icon);
        m_BackGround.SetSprite(2);

        for (var i = 0; i < m_Stars.Length; i++)
        {
            m_Stars[i].gameObject.SetActive(i < config.starLevel);
        }
    }

    private void DisplayCountInfo()
    {
        if (m_CountContainer != null)
        {
            m_CountContainer.gameObject.SetActive(this.count > 1);
            if (this.count > 1)
            {
                m_Count.SetText(this.count);
            }
        }
    }

    private void OnClick()
    {
        if (onClick != null)
        {
            onClick();
        }
        else
        {
            ViewItemInfo();
        }
    }

    private void ViewItemInfo()
    {
        ItemInfo.Instance.ViewItemInfo(new Item()
        {
            instanceId = this.instanceId,
            id = this.id,
            count = this.count
        });
    }


}



