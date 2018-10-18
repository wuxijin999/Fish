using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDRoot : MonoBehaviour
{
    [SerializeField] Canvas[] m_BloodNum;
    [SerializeField] Canvas m_HeadUpBar;
    public Canvas headUpBar { get { return m_HeadUpBar; } }

    public Canvas PickBloodCanvas()
    {
        foreach (var item in m_BloodNum)
        {
            if (item.transform.childCount < 20)
            {
                return item;
            }
        }

        return m_BloodNum.GetLast();
    }

}
