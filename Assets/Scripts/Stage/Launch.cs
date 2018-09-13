using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Launch : MonoBehaviour
{

    private void Awake()
    {
        ModelUtil.Init();
        ActorEngine.Instance.Launch();

        DebugEx.LogFormat("配置加载开始：{0}",DateTime.Now);
        ConfigInitiator.Init();

    }

}
