using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VesselExtension
{
    public static void AddEx<T>(this List<T> list, T value)
    {
        if (list != null && !list.Contains(value))
        {
            list.Add(value);
        }
    }

}
