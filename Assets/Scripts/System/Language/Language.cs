using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language
{
    public static string Get(int id)
    {
        var config = LanguageConfig.Get(id);
        return config == null ? string.Empty : config.content;
    }

    public static string GetFormat(int id, params object[] objects)
    {
        try
        {
            var config = LanguageConfig.Get(id);
            if (config != null)
            {
                return string.Format(config.content, objects);
            }
            else
            {
                return string.Empty;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            return string.Empty;
        }
    }

    public static string GetLocal(int id)
    {
        var config = PriorLanguageConfig.Get(id);
        return config == null ? string.Empty : config.content;
    }

    public static string GetLocalFormat(int id, params object[] objects)
    {
        try
        {
            var config = PriorLanguageConfig.Get(id);
            if (config != null)
            {
                return string.Format(config.content, objects);
            }
            else
            {
                return string.Empty;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            return string.Empty;
        }
    }

    static string m_CurrentLanguage = string.Empty;
    public static string currentLanguage {
        get { return m_CurrentLanguage; }
        private set { m_CurrentLanguage = value; }
    }

    public static void SwitchLanguage(string language)
    {
        currentLanguage = language;
    }

}
