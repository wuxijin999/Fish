//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class HorseConfig
{

    public readonly int HorseID;
	public readonly int ItemID;
	public readonly string Name;
	public readonly int UnlockItemID;
	public readonly int UnlockItemCnt;
	public readonly int InitLV;
	public readonly int MaxLV;
	public readonly int UseNeedRank;
	public readonly int Model;
	public readonly string IconKey;
	public readonly int ActionType;
	public readonly int Quality;
	public readonly int InitFightPower;
	public readonly int ShowFightPower;
	public readonly int Sort;

    public HorseConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out HorseID); 

			int.TryParse(tables[1],out ItemID); 

			Name = tables[2];

			int.TryParse(tables[3],out UnlockItemID); 

			int.TryParse(tables[4],out UnlockItemCnt); 

			int.TryParse(tables[5],out InitLV); 

			int.TryParse(tables[6],out MaxLV); 

			int.TryParse(tables[7],out UseNeedRank); 

			int.TryParse(tables[8],out Model); 

			IconKey = tables[9];

			int.TryParse(tables[10],out ActionType); 

			int.TryParse(tables[11],out Quality); 

			int.TryParse(tables[12],out InitFightPower); 

			int.TryParse(tables[13],out ShowFightPower); 

			int.TryParse(tables[14],out Sort); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, HorseConfig> configs = new Dictionary<int, HorseConfig>();
    public static HorseConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        HorseConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new HorseConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Horse.txt";
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

			DebugEx.LogFormat("加载结束HorseConfig：{0}",   DateTime.Now);
        });
    }

}




