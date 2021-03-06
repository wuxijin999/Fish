﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRole : Presenter<CreateRole>, IPresenterWindow
{

    BrowseJob browseJob = new BrowseJob();
    public readonly IntProperty browsingJob = new IntProperty(1);
    public readonly IntProperty browsingGender = new IntProperty(0);
    public readonly StringProperty randomName = new StringProperty();

    public void OpenWindow(object @object)
    {
    }

    public void CloseWindow()
    {
        Windows.Instance.Close(WindowType.CreateRole);
    }

    public void ViewJob(int job)
    {
        browsingJob.value = job;
        randomName.value = GetRandomName();
    }

    public void ViewGender(int gender)
    {
        browsingGender.value = gender;
        randomName.value = GetRandomName();
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
        return this.browseJob.Random();
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
            this.job = 1;
        }

        public int Random()
        {
            return UnityEngine.Random.Range(1, 4);
        }

    }



}





