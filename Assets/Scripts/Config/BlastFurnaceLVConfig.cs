﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class BlastFurnaceLVConfig
{

    public readonly int BlastFurnaceLV;
	public readonly int BlastFurnaceEXP;

    public BlastFurnaceLVConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out BlastFurnaceLV); 

			int.TryParse(tables[1],out BlastFurnaceEXP); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, BlastFurnaceLVConfig> configs = new Dictionary<int, BlastFurnaceLVConfig>();
    public static BlastFurnaceLVConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        BlastFurnaceLVConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new BlastFurnaceLVConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "BlastFurnaceLV.txt";
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

			DebugEx.LogFormat("加载结束BlastFurnaceLVConfig：{0}",   DateTime.Now);
        });
    }

}




