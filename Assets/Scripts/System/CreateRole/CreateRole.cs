//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRole : Presenter<CreateRole>
{

    public BizEvent browseJobEvent = new BizEvent();

    BrowseJob browseJob = new BrowseJob();

    public int browsingJob {
        get { return browseJob.job; }
        set {
            if (browseJob.job != value)
            {
                browseJob.job = value;
                browseJobEvent.Invoke();
            }
        }
    }

    public override void OpenWindow()
    {
        browseJob.Reset();
        Windows.Instance.Open(WindowType.CreateRole);
    }

    public override void CloseWindow()
    {
        Windows.Instance.Close(WindowType.CreateRole);
    }

    public void Create(int job, string name)
    {
        if (!job.InRange(1, 4))
        {
            return;
        }

        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        if (!IsValidRoleName(name))
        {
            return;
        }

    }

    public int GetRandomJob()
    {
        return browseJob.Random();
    }

    public string GetRandomName()
    {
        return string.Empty;
    }

    public bool IsValidRoleName(string name)
    {


        return true;
    }

    public class BrowseJob
    {
        public int job = 1;

        public void Reset()
        {
            job = 1;
        }

        public int Random()
        {
            return UnityEngine.Random.Range(1, 4);
        }

    }



}





