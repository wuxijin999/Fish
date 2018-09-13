//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateRoleJobSelectWidget : Widget
{
    [SerializeField] CreateRoleJobSelectButton m_Warrior;
    [SerializeField] CreateRoleJobSelectButton m_Wizard;
    [SerializeField] CreateRoleJobSelectButton m_Pastor;

    protected override void SetListeners()
    {
        m_Warrior.SetListener(() => { CreateRole.Instance.browsingJob = 1; });
        m_Wizard.SetListener(() => { CreateRole.Instance.browsingJob = 2; });
        m_Pastor.SetListener(() => { CreateRole.Instance.browsingJob = 3; });
    }

    protected override void OnActived()
    {
        CreateRole.Instance.browseJobEvent += OnSelectJob;
    }

    protected override void OnDeactived()
    {
        CreateRole.Instance.browseJobEvent -= OnSelectJob;
    }

    private void OnSelectJob()
    {
        m_Warrior.selected = CreateRole.Instance.browsingJob == 1;
        m_Wizard.selected = CreateRole.Instance.browsingJob == 2;
        m_Pastor.selected = CreateRole.Instance.browsingJob == 3;
    }

}



