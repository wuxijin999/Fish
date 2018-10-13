﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DungeonConfig
{

    public readonly int id;
	public readonly int name;
	public readonly int description;
	public readonly int mapId;
	public readonly int boss;
	public readonly int levelMin;
	public readonly Int2 recommendLevel;
	public readonly int timeLimit;
	public readonly Int2[] rewards;

    public DungeonConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out name); 

			int.TryParse(tables[2],out description); 

			int.TryParse(tables[3],out mapId); 

			int.TryParse(tables[4],out boss); 

			int.TryParse(tables[5],out levelMin); 

			Int2.TryParse(tables[6],out recommendLevel); 

			int.TryParse(tables[7],out timeLimit); 

			string[] rewardsStringArray = tables[8].Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			rewards = new Int2[rewardsStringArray.Length];
			for (int i=0;i<rewardsStringArray.Length;i++)
			{
				 Int2.TryParse(rewardsStringArray[i],out rewards[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DungeonConfig> configs = new Dictionary<int, DungeonConfig>();
    public static DungeonConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        DungeonConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new DungeonConfig(rawDatas[id]);
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
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Dungeon.txt";
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




