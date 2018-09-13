using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Widget : UIBase
{

    bool inited = false;
    protected virtual void BindControllers()
    {

    }

    protected virtual void SetListeners()
    {

    }

    protected virtual void OnActived() { }
    protected virtual void OnDeactived() { }

    public void SetActive(bool active)
    {
        if (active)
        {
            if (!inited)
            {
                BindControllers();
                SetListeners();
                inited = true;
            }

            this.gameObject.SetActive(true);
            OnActived();
        }
        else
        {
            OnDeactived();
            this.gameObject.SetActive(false);
        }
    }



}
