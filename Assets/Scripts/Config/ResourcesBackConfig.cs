//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ResourcesBackConfig
{

    public readonly int ID;
	public readonly int RelatedID;
	public readonly int CanBackTimes;
	public readonly int NormalCostJade;
	public readonly int VipCostJade;
	public readonly string JadeReward;
	public readonly int CostCopper;
	public readonly string CopperReward;
	public readonly string RewardList;

    public ResourcesBackConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out RelatedID); 

			int.TryParse(tables[2],out CanBackTimes); 

			int.TryParse(tables[3],out NormalCostJade); 

			int.TryParse(tables[4],out VipCostJade); 

			JadeReward = tables[5];

			int.TryParse(tables[6],out CostCopper); 

			CopperReward = tables[7];

			RewardList = tables[8];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ResourcesBackConfig> configs = new Dictionary<int, ResourcesBackConfig>();
    public static ResourcesBackConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ResourcesBackConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ResourcesBackConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ResourcesBack.txt";
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

			DebugEx.LogFormat("加载结束ResourcesBackConfig：{0}",   DateTime.Now);
        });
    }

}




