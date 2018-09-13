using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Widget : MonoBehaviour
{

    protected GameObject instance;
    protected virtual void BindControllers()
    {

    }
    protected virtual void SetListeners()
    {

    }

    public void SetActive(bool active)
    {
        if (active)
        {
            if (instance == null)
            {
                UIAssets.LoadWindowAsync(this.name, OnLoad);
            }
            else
            {
                instance.SetActive(true);
            }
        }
        else
        {
            instance.SetActive(false);
        }
    }

    private void OnLoad(bool ok, UnityEngine.Object @object)
    {
        if (ok && @object != null)
        {
            var prefab = @object as GameObject;
            instance = GameObject.Instantiate(prefab);
            UIAssets.UnLoadWindowAsset(this.name);
            BindControllers();
            SetListeners();
            instance.SetActive(true);
        }
        else
        {
            DebugEx.LogFormat("{0}资源不存在，请检查！", this.name);
        }
    }

}
