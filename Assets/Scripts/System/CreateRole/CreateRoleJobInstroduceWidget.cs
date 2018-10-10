//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class CreateRoleJobInstroduceWidget : Widget
{
    [SerializeField] TextEx m_JobName;
    [SerializeField] TextEx m_Instroduce;
    [SerializeField] ImageEx m_Icon;
    [SerializeField] TextEx m_Speciality;

    protected override void SetListeners()
    {

    }

    public void Display(int job)
    {

    }

}



