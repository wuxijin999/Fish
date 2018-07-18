//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class UnionLivenConfig
{

    public readonly int ID;
	public readonly string Name;
	public readonly int UnLockFuncID;
	public readonly int OnceActivityTime;
	public readonly int TotalActiveValue;
	public readonly string RewardDes;
	public readonly string IconName;

    public UnionLivenConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			Name = tables[1];

			int.TryParse(tables[2],out UnLockFuncID); 

			int.TryParse(tables[3],out OnceActivityTime); 

			int.TryParse(tables[4],out TotalActiveValue); 

			RewardDes = tables[5];

			IconName = tables[6];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, UnionLivenConfig> configs = new Dictionary<int, UnionLivenConfig>();
    public static UnionLivenConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        UnionLivenConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new UnionLivenConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "UnionLiven.txt";
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

			DebugEx.LogFormat("加载结束UnionLivenConfig：{0}",   DateTime.Now);
        });
    }

}




