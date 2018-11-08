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

    protected virtual void OnBeforeActived() { }
    protected virtual void OnActived() { }
    protected virtual void OnDeactived() { }

    public void SetActive(bool active)
    {
        if (active)
        {
            if (!this.inited)
            {
                BindControllers();
                SetListeners();
                this.inited = true;
            }

            OnBeforeActived();
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
