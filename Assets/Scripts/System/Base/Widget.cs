using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Widget : MonoBehaviour
{
    public abstract void AddListeners();
    public abstract void BindControllers();

    public void Load()
    {

    }

    public void UnLoad()
    {

    }

}
