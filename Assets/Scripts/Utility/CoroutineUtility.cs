using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtility : SingletonMonobehaviour<CoroutineUtility>
{

    public void Coroutine(IEnumerator _coroutine)
    {
        StartCoroutine(_coroutine);
    }


}
