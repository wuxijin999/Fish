//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class LVAawrdConfig
{

    public readonly int RewardID;
	public readonly int LV;
	public readonly int LimitNUM;
	public readonly string Reward;
	public readonly int VIPLimit;
	public readonly string VIPAward;

    public LVAawrdConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out RewardID); 

			int.TryParse(tables[1],out LV); 

			int.TryParse(tables[2],out LimitNUM); 

			Reward = tables[3];

			int.TryParse(tables[4],out VIPLimit); 

			VIPAward = tables[5];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, LVAawrdConfig> configs = new Dictionary<int, LVAawrdConfig>();
    public static LVAawrdConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        LVAawrdConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new LVAawrdConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "LVAawrd.txt";
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

			DebugEx.LogFormat("加载结束LVAawrdConfig：{0}",   DateTime.Now);
        });
    }

}




