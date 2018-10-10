using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : Scene
{

    public override void OnInitialize()
    {
        Login.Instance.OpenWindow();
    }

    public override void OnUnInitialize()
    {
        Login.Instance.CloseWindow();
    }

}
