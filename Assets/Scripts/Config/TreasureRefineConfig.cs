//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TreasureRefineConfig
{

    public readonly int id;
	public readonly int TreasureID;
	public readonly int TreasureLV;
	public readonly string TreasureLVStatus;
	public readonly string TreasureUpItems;
	public readonly int SuccessRate;
	public readonly int OpenSkill;
	public readonly int BlastFurnaceLV;
	public readonly int AllTreasureLV;
	public readonly int IsCoreSkill;

    public TreasureRefineConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out TreasureID); 

			int.TryParse(tables[2],out TreasureLV); 

			TreasureLVStatus = tables[3];

			TreasureUpItems = tables[4];

			int.TryParse(tables[5],out SuccessRate); 

			int.TryParse(tables[6],out OpenSkill); 

			int.TryParse(tables[7],out BlastFurnaceLV); 

			int.TryParse(tables[8],out AllTreasureLV); 

			int.TryParse(tables[9],out IsCoreSkill); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TreasureRefineConfig> configs = new Dictionary<int, TreasureRefineConfig>();
    public static TreasureRefineConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TreasureRefineConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TreasureRefineConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TreasureRefine.txt";
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

			DebugEx.LogFormat("加载结束TreasureRefineConfig：{0}",   DateTime.Now);
        });
    }

}




