using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtil : SingletonMonobehaviour<CoroutineUtil>
{

    Dictionary<int, Dictionary<string, Coroutine>> coroutines = new Dictionary<int, Dictionary<string, Coroutine>>();

    public void Begin(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void Begin(int hash, string methodName, IEnumerator routine)
    {
        Stop(hash, methodName);
        var coroutine = StartCoroutine(routine);
        var table = coroutines.ContainsKey(hash) ? coroutines[hash] : coroutines[hash] = new Dictionary<string, Coroutine>();
        table[methodName] = coroutine;
    }

    public void Stop(int hash, string methodName)
    {
        if (coroutines.ContainsKey(hash))
        {
            var table = coroutines[hash];
            if (table.ContainsKey(methodName))
            {
                StopCoroutine(table[methodName]);
                table.Remove(methodName);
            }
        }
    }

    public void StopAll()
    {
        foreach (var table in coroutines.Values)
        {
            foreach (var item in table.Values)
            {
                StopCoroutine(item);
            }
        }

        coroutines.Clear();
    }

    private void OnDestroy()
    {
        StopAll();
    }


}
