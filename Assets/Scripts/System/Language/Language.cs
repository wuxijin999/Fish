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


}
