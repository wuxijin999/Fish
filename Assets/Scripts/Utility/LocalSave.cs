//--------------------------------------------------------
//    [Author]:                   Wu Xijin
//    [Date]   :           Sunday, May 08, 2016
//--------------------------------------------------------
using UnityEngine;
using System.Text;
using System;

public static class LocalSave
{

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteKey(string _key)
    {
        PlayerPrefs.DeleteKey(_key);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key, int _default = 0)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return _default;
        }
        else
        {
            return PlayerPrefs.GetInt(key);
        }
    }

    public static void SetFloat(string key, float value)
    {

        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key, float _default = 0f)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return _default;
        }
        else
        {
            return PlayerPrefs.GetFloat(key);
        }
    }

    public static void SetBool(string key, bool value)
    {

        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key, bool _default = false)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return _default;
        }
        else
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
    }

    public static void SetString(string key, string value)
    {

        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return string.Empty;
        }
        else
        {
            return PlayerPrefs.GetString(key);
        }
    }

    public static void SetVector3(string key, Vector3 value)
    {
        var sb = new StringBuilder();
        sb.Append(value.x);
        sb.Append(";");
        sb.Append(value.y);
        sb.Append(";");
        sb.Append(value.z);

        PlayerPrefs.SetString(key, sb.ToString());
    }

    public static Vector3 GetVector3(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return Vector3.zero;
        }
        else
        {
            var v = new Vector3();
            var strArray = PlayerPrefs.GetString(key).Split(';');
            v.x = float.Parse(strArray[0]);
            v.y = float.Parse(strArray[2]);
            v.z = float.Parse(strArray[4]);

            return v;
        }
    }

    public static void SetIntArray(string key, int[] value)
    {
        if (value != null && value.Length > 0)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                sb.Append(';');
            }

            sb.Remove(sb.Length - 1, 1);
            PlayerPrefs.SetString(key, sb.ToString());
        }
        else
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    public static int[] GetIntArray(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return null;
        }
        else
        {
            var value = PlayerPrefs.GetString(key);

            var strArray = value.Split(';');
            var intArray = new int[strArray.Length];
            for (var i = 0; i < strArray.Length; i++)
            {
                intArray[i] = int.Parse(strArray[i]);
            }

            return intArray;
        }
    }

    public static void SetFloatArray(string key, float[] value)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < value.Length; i++)
        {
            sb.Append(value[i]);
            sb.Append(";");
        }

        sb.Remove(sb.Length - 1, 1);

        PlayerPrefs.SetString(key, sb.ToString());
    }

    public static float[] GetFloatArray(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return null;
        }
        else
        {
            var value = PlayerPrefs.GetString(key);
            var strArray = value.Split(';');
            var array = new float[strArray.Length];
            for (var i = 0; i < strArray.Length; i++)
            {
                array[i] = float.Parse(strArray[i]);
            }

            return array;
        }

    }

    public static void SetStringArray(string key, string[] value)
    {
        var valueGroup = string.Join(";", value);
        PlayerPrefs.SetString(key, valueGroup);
    }

    public static string[] GeStringArray(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return null;
        }
        else
        {
            var value = PlayerPrefs.GetString(key);
            return value.Split(';');
        }
    }
}



