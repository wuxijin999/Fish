using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountLocalSave
{

    public static string accountId;

    public static string GetAmendedKey(string key)
    {
        return StringUtil.Contact(accountId + key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void SetInt(string key, int value)
    {
        LocalSave.SetInt(GetAmendedKey(key), value);
    }

    public static int GetInt(string key, int @default = 0)
    {
        return LocalSave.GetInt(GetAmendedKey(key), @default);
    }

    public static void SetFloat(string key, float value)
    {
        LocalSave.SetFloat(GetAmendedKey(key), value);
    }

    public static float GetFloat(string key, float @default = 0f)
    {
        return LocalSave.GetFloat(GetAmendedKey(key), @default);
    }

    public static void SetBool(string key, bool value)
    {
        LocalSave.SetBool(GetAmendedKey(key), value);
    }

    public static bool GetBool(string key, bool @default = false)
    {
        return LocalSave.GetBool(GetAmendedKey(key), @default);
    }

    public static void SetString(string key, string value)
    {
        LocalSave.SetString(GetAmendedKey(key), value);
    }

    public static string GetString(string key)
    {
        return LocalSave.GetString(GetAmendedKey(key));
    }

    public static void SetVector3(string key, Vector3 value)
    {
        LocalSave.SetVector3(GetAmendedKey(key), value);
    }

    public static Vector3 GetVector3(string key)
    {
        return LocalSave.GetVector3(GetAmendedKey(key));
    }

    public static void SetIntArray(string key, int[] value)
    {
        LocalSave.SetIntArray(GetAmendedKey(key), value);
    }

    public static int[] GetIntArray(string key)
    {
        return LocalSave.GetIntArray(GetAmendedKey(key));
    }

    public static void SetFloatArray(string key, float[] value)
    {
        LocalSave.SetFloatArray(GetAmendedKey(key), value);
    }

    public static float[] GetFloatArray(string key)
    {
        return LocalSave.GetFloatArray(GetAmendedKey(key));
    }

    public static void SetStringArray(string key, string[] value)
    {
        LocalSave.SetStringArray(GetAmendedKey(key), value);
    }

    public static string[] GetStringArray(string key)
    {
        return LocalSave.GetStringArray(key);
    }

}
