using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEx
{
    public static bool EnableLog = false;
    public static bool EnableLogWarning = false;
    public static bool EnableLogError = false;
    public static bool EnableNetLog = false;

    public static void Init()
    {
        EnableLog = LocalSave.GetBool("DesignEnableLog", false);
        EnableLogWarning = LocalSave.GetBool("DesignEnableLogWarning", false);
        EnableLogError = LocalSave.GetBool("DesignEnableLogError", false);
        EnableNetLog = LocalSave.GetBool("DesignEnableNetLog", false);
    }

    public static void SetLogAble(bool _able)
    {
        LocalSave.SetBool("DesignEnableLog", _able);
        EnableLog = _able;
    }

    public static void SetLogWarningAble(bool _able)
    {
        LocalSave.SetBool("DesignEnableLogWarning", _able);
        EnableLogWarning = _able;
    }

    public static void SetLogErrorAble(bool _able)
    {
        LocalSave.SetBool("DesignEnableLogError", _able);
        EnableLogError = _able;
    }

    public static void SetNetLogAble(bool _able)
    {
        LocalSave.SetBool("DesignEnableNetLog", _able);
        EnableNetLog = _able;
    }

    public static void Log(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.Log(message, context);
        }
    }

    public static void Log(object message)
    {
        if (EnableLog)
        {
            Debug.Log(message);
        }
    }

    public static void LogFormat(string message, params object[] _objs)
    {
        if (EnableLog)
        {
            Debug.LogFormat(message, _objs);
        }
    }

    public static void LogError(object message, Object context)
    {
        if (EnableLogError)
        {
            Debug.LogError(message, context);
        }
    }

    public static void LogError(object message)
    {
        if (EnableLogError)
        {
            Debug.LogError(message);
        }
    }

    public static void LogErrorFormat(string message, params object[] _objs)
    {
        if (EnableLogError)
        {
            Debug.LogErrorFormat(message, _objs);
        }
    }

    public static void LogWarning(object message, Object context)
    {
        if (EnableLogWarning)
        {
            Debug.LogWarning(message, context);
        }
    }

    public static void LogWarning(object message)
    {
        if (EnableLogWarning)
        {
            Debug.LogWarning(message);
        }
    }

    public static void LogWarningFormat(string message, params object[] _objs)
    {
        if (EnableLogWarning)
        {
            Debug.LogWarningFormat(message, _objs);
        }
    }

    public static void NetLog(object message, Object context)
    {
        if (EnableNetLog)
        {
            Debug.Log(message, context);
        }
    }

    public static void NetLog(object message)
    {
        if (EnableNetLog)
        {
            Debug.Log(message);
        }
    }

    public static void NetLogFormat(string message, params object[] _objs)
    {
        if (EnableNetLog)
        {
            Debug.LogFormat(message, _objs);
        }
    }

}
