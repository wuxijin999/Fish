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

    #region Built-in

    protected override void SetListeners()
    {
        this.m_RandomName.SetListener(this.RandomName);
        this.m_Create.SetListener(this.Create);
    }

    protected override void OnPreOpen()
    {
        this.m_RoleName.text = string.Empty;
    }

    protected override void OnActived()
    {
        base.OnActived();
        RandomName();
        SetWidgetActive<CreateRoleJobSelectWidget>(true);
        SetWidgetActive<CreateRoleJobInstroduceWidget>(true);
    }

    protected override void OnPreClose()
    {
        SetWidgetActive<CreateRoleJobSelectWidget>(false);
        SetWidgetActive<CreateRoleJobInstroduceWidget>(false);
    }

    #endregion

    private void RandomName()
    {
        this.m_RoleName.text = CreateRole.Instance.GetRandomName();
    }

    private void Create()
    {
        var job = CreateRole.Instance.browsingJob;
        var name = this.m_RoleName.text;
        CreateRole.Instance.Create(job, name);
    }


}





