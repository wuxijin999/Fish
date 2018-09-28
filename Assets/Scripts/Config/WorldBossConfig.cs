//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class WorldBossConfig
{

    public readonly int id;
	public readonly int level;
	public readonly string name;
	public readonly int[] rewards;
	public readonly int icon;
	public readonly int mapId;

    public WorldBossConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out level); 

			name = tables[2];

			string[] rewardsStringArray = tables[3].Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			rewards = new int[rewardsStringArray.Length];
			for (int i=0;i<rewardsStringArray.Length;i++)
			{
				 int.TryParse(rewardsStringArray[i],out rewards[i]);
			}

			int.TryParse(tables[4],out icon); 

			int.TryParse(tables[5],out mapId); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, WorldBossConfig> configs = new Dictionary<int, WorldBossConfig>();
    public static WorldBossConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        WorldBossConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new WorldBossConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "WorldBoss.txt";
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




