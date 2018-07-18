//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RuneTowerFloorConfig
{

    public readonly int ID;
	public readonly int TowerId;
	public readonly int FloorIndex;
	public readonly string FloorName;
	public readonly string RewardTitle;
	public readonly int RuneEssence;
	public readonly int RuneMagicEssence;
	public readonly int BossId;
	public readonly int UnLockRune;
	public readonly int UnLockHole;
	public readonly int UnLockCompose;
	public readonly int RuneDrop;

    public RuneTowerFloorConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out TowerId); 

			int.TryParse(tables[2],out FloorIndex); 

			FloorName = tables[3];

			RewardTitle = tables[4];

			int.TryParse(tables[5],out RuneEssence); 

			int.TryParse(tables[6],out RuneMagicEssence); 

			int.TryParse(tables[7],out BossId); 

			int.TryParse(tables[8],out UnLockRune); 

			int.TryParse(tables[9],out UnLockHole); 

			int.TryParse(tables[10],out UnLockCompose); 

			int.TryParse(tables[11],out RuneDrop); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RuneTowerFloorConfig> configs = new Dictionary<int, RuneTowerFloorConfig>();
    public static RuneTowerFloorConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RuneTowerFloorConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RuneTowerFloorConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "RuneTowerFloor.txt";
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

			DebugEx.LogFormat("加载结束RuneTowerFloorConfig：{0}",   DateTime.Now);
        });
    }

}




