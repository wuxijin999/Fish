//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PriorLanguageConfig
{

    public readonly int id;
    public readonly string content;

    public PriorLanguageConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0], out id);

            content = tables[1];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PriorLanguageConfig> configs = new Dictionary<int, PriorLanguageConfig>();
    public static PriorLanguageConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        PriorLanguageConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new PriorLanguageConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }

    public static bool Has(int id)
    {
        return configs.ContainsKey(id);
    }

    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = Application.dataPath + Path.DirectorySeparatorChar + "Resources/PriorLanguage.txt";
        var lines = File.ReadAllLines(path);
        rawDatas = new Dictionary<int, string>(lines.Length - 3);
        for (int i = 3; i < lines.Length; i++)
        {
            var line = lines[i];
            var index = line.IndexOf("\t");
            var idString = line.Substring(0, index);
            var id = int.Parse(idString);

            rawDatas[id] = line;
        }
    }

}




