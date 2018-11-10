using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class Launch : MonoBehaviour
{

    private void Awake()
    {
        ConfigInitiator.PreInit();
    }

    void Start()
    {
        StartCoroutine(Co_Launch());
    }

    IEnumerator Co_Launch()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (AssetsCopyTask.copiedVersion != VersionConfig.Get().version)
            {
                var assetCopy = AssetsCopyTask.BeginCopy(VersionConfig.Get().version);
                while (!assetCopy.done)
                {
                    yield return null;
                }
            }
        }

        ConfigInitiator.Init();

        yield return new WaitForSeconds(1f);
        SceneLoad.Instance.LoadLogin();
    }

}
