using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtil : SingletonMonobehaviour<CoroutineUtil>
{

    public void Coroutine(IEnumerator _coroutine)
    {
        StartCoroutine(_coroutine);
    }


}
