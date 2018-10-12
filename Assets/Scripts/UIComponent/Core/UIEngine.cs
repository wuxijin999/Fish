using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEngine : MonoBehaviour
{
    public static UIEngine Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        if (FindObjectOfType<UIEngine>() == null)
        {
            var gameObject = new GameObject("UIEngine");
            Instance = gameObject.AddComponent<UIEngine>();
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    List<UIBase> uibases = new List<UIBase>();

    public void Register(UIBase uibase)
    {
        this.uibases.AddEx(uibase);
    }

    public void UnRegister(UIBase uibase)
    {
        this.uibases.Remove(uibase);
    }

    private void Update()
    {
        foreach (var item in this.uibases)
        {
            try
            {
                if (item != null && item.isActiveAndEnabled)
                {
                    item.OnUpdate();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    private void LateUpdate()
    {
        foreach (var item in this.uibases)
        {
            try
            {
                if (item != null && item.isActiveAndEnabled)
                {
                    item.OnLateUpdate();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

}
