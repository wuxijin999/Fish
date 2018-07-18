//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PetClassCostConfig
{

    public readonly int ID;
	public readonly int PetID;
	public readonly int Rank;
	public readonly int UpNeedExp;
	public readonly int AtkAdd;

    public PetClassCostConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out PetID); 

			int.TryParse(tables[2],out Rank); 

			int.TryParse(tables[3],out UpNeedExp); 

			int.TryParse(tables[4],out AtkAdd); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PetClassCostConfig> configs = new Dictionary<int, PetClassCostConfig>();
    public static PetClassCostConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        PetClassCostConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new PetClassCostConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "PetClassCost.txt";
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

			DebugEx.LogFormat("加载结束PetClassCostConfig：{0}",   DateTime.Now);
        });
    }

}




