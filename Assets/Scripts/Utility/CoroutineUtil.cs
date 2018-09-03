using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtil : SingletonMonobehaviour<CoroutineUtil>
{

    public void Coroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }


}
