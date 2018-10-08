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
        this.m_Warrior.SetListener(() => { CreateRole.Instance.ViewJob(1); });
        this.m_Wizard.SetListener(() => { CreateRole.Instance.ViewJob(2); });
        this.m_Pastor.SetListener(() => { CreateRole.Instance.ViewJob(3); });
    }

    protected override void OnActived()
    {
        DisplaySelectJob();
    }

    protected override void OnDeactived()
    {
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (CreateRole.Instance.browsingJob.dirty)
        {
            DisplaySelectJob();
        }
    }

    private void DisplaySelectJob()
    {
        var selectedJob = CreateRole.Instance.browsingJob.Fetch();
        this.m_Warrior.selected = selectedJob == 1;
        this.m_Wizard.selected = selectedJob == 2;
        this.m_Pastor.selected = selectedJob == 3;
    }

}



