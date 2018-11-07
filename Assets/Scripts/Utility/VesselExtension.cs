using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class VesselExtension
{
    public static void AddEx<T>(this List<T> list, T value)
    {
        if (list != null && !list.Contains(value))
        {
            list.Add(value);
        }
    }

    public static T GetFirst<T>(this List<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException("List is null.");
        }

        if (list.Count == 0)
        {
            Debug.Log("List count can't be zero.");
            return default(T);
        }

        return list[0];
    }

    public static T GetLast<T>(this List<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException("List is null.");
        }

        if (list.Count == 0)
        {
            Debug.Log("List count can't be zero.");
            return default(T);
        }

        return list[list.Count - 1];
    }

    public static T GetFirst<T>(this T[] list)
    {
        if (list == null)
        {
            throw new ArgumentNullException("Array is null.");
        }

        if (list.Length == 0)
        {
            Debug.Log("Array count can't be zero.");
            return default(T);
        }

        return list[0];
    }

    public static T GetLast<T>(this T[] list)
    {
        if (list == null)
        {
            throw new ArgumentNullException("Array is null.");
        }

        if (list.Length == 0)
        {
            Debug.Log("Array count can't be zero.");
            return default(T);
        }

        return list[list.Length - 1];
    }

}


