//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class WorldBossConfig
{

    public readonly int NPCID;
	public readonly int[] RareItemID;
	public readonly int MapID;
	public readonly string PortraitID;

    public WorldBossConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out NPCID); 

			string[] RareItemIDStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RareItemID = new int[RareItemIDStringArray.Length];
			for (int i=0;i<RareItemIDStringArray.Length;i++)
			{
				 int.TryParse(RareItemIDStringArray[i],out RareItemID[i]);
			}

			int.TryParse(tables[2],out MapID); 

			PortraitID = tables[3];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, WorldBossConfig> configs = new Dictionary<int, WorldBossConfig>();
    public static WorldBossConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        WorldBossConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new WorldBossConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
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

			DebugEx.LogFormat("加载结束WorldBossConfig：{0}",   DateTime.Now);
        });
    }

}




