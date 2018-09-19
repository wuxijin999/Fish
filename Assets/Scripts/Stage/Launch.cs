using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Actor;

public class Launch : MonoBehaviour
{

    private void Awake()
    {
        DebugEx.LogFormat("配置加载开始：{0}", DateTime.Now);
        ConfigInitiator.Init();

        var actor = ActorCenter.Instance.Create(null);

    }

}
