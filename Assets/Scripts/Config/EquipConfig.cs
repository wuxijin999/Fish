﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, November 05, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EquipConfig
{

    public readonly int id;
	public readonly int place;

    public EquipConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out place); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EquipConfig> configs = new Dictionary<int, EquipConfig>();
    public static EquipConfig Get(int id)
    {   
		if (!inited)
        {
            Debug.Log("EquipConfigConfig 还未完成初始化。");
            return null;
        }
		
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        EquipConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new EquipConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }

	public static bool Has(int id)
    {
        return configs.ContainsKey(id);
    }

	static bool inited = false;
    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
	    inited = false;
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Equip.txt";
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

			inited=true;
        });
    }

}




