//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DungeonInspireConfig
{

    public readonly int ID;
	public readonly int DataMapId;
	public readonly int InspireType;
	public readonly int InspireCount;
	public readonly int MoneyCount;

    public DungeonInspireConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out DataMapId); 

			int.TryParse(tables[2],out InspireType); 

			int.TryParse(tables[3],out InspireCount); 

			int.TryParse(tables[4],out MoneyCount); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DungeonInspireConfig> configs = new Dictionary<int, DungeonInspireConfig>();
    public static DungeonInspireConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DungeonInspireConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DungeonInspireConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DungeonInspire.txt";
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

			DebugEx.LogFormat("加载结束DungeonInspireConfig：{0}",   DateTime.Now);
        });
    }

}




