using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEngine : SingletonMonobehaviour<UIEngine>
{
    List<UIBase> uibases = new List<UIBase>();

    public void Register(UIBase uibase)
    {
        if (!uibases.Contains(uibase))
        {
            uibases.Add(uibase);
        }
    }

    public void UnRegister(UIBase uibase)
    {
        if (uibases.Contains(uibase))
        {
            uibases.Remove(uibase);
        }
    }

    private void Update()
    {
        foreach (var item in uibases)
        {
            try
            {
                item.OnUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    private void LateUpdate()
    {
        foreach (var item in uibases)
        {
            try
            {
                item.OnLateUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

}
