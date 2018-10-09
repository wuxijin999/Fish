//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Tuesday, October 09, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class IconConfig
{

    public readonly int id;
	public readonly string folder;
	public readonly string assetName;

    public IconConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			folder = tables[1];

			assetName = tables[2];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, IconConfig> configs = new Dictionary<int, IconConfig>();
    public static IconConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        IconConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new IconConfig(rawDatas[id]);
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
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Icon.txt";
        ThreadPool.QueueUserWorkItem((object _object) =>
        {
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
        });
    }

}




