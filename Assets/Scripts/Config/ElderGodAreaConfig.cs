//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ElderGodAreaConfig
{

    public readonly int NPCID;
	public readonly int MonsterType;
	public readonly int MonsterAnger;
	public readonly int[] RareItemID;
	public readonly string PortraitID;

    public ElderGodAreaConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out NPCID); 

			int.TryParse(tables[1],out MonsterType); 

			int.TryParse(tables[2],out MonsterAnger); 

			string[] RareItemIDStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RareItemID = new int[RareItemIDStringArray.Length];
			for (int i=0;i<RareItemIDStringArray.Length;i++)
			{
				 int.TryParse(RareItemIDStringArray[i],out RareItemID[i]);
			}

			PortraitID = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ElderGodAreaConfig> configs = new Dictionary<int, ElderGodAreaConfig>();
    public static ElderGodAreaConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ElderGodAreaConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ElderGodAreaConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ElderGodArea.txt";
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

			DebugEx.LogFormat("加载结束ElderGodAreaConfig：{0}",   DateTime.Now);
        });
    }

}




