using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour
{

    private void Awake()
    {
        ModelUtility.Init();
        ActorEngine.Instance.Launch();

        ItemConfig.Init();
    }

}
