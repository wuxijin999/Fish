//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoleWin : Window
{

    [SerializeField] InputField m_RoleName;
    [SerializeField] ButtonEx m_RandomName;
    [SerializeField] ButtonEx m_Create;

    [SerializeField] CreateRoleJobInstroduceWidget m_JobInstroduce;

    #region Built-in

    protected override void SetListeners()
    {
        m_RandomName.SetListener(RandomName);
        m_Create.SetListener(Create);
    }

    protected override void OnPreOpen()
    {
        m_RoleName.text = string.Empty;
    }

    protected override void OnPreClose()
    {
    }

    protected override void OnActived()
    {
        base.OnActived();
        RandomName();
        m_JobInstroduce.SetActive(true);
    }

    #endregion

    private void RandomName()
    {
        m_RoleName.text = CreateRole.Instance.GetRandomName();
    }

    private void Create()
    {
        var job = CreateRole.Instance.browsingJob;
        var name = m_RoleName.text;
        CreateRole.Instance.Create(job, name);
    }


}





