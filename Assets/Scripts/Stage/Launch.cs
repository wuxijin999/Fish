using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{

    private void Awake()
    {
        ModelUtility.Init();
        ActorEngine.Instance.Launch();
    }

}
