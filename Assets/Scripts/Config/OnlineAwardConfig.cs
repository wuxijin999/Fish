//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class OnlineAwardConfig
{

    public readonly int RewardID;
	public readonly int[] Time;
	public readonly string Reward;

    public OnlineAwardConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out RewardID); 

			string[] TimeStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Time = new int[TimeStringArray.Length];
			for (int i=0;i<TimeStringArray.Length;i++)
			{
				 int.TryParse(TimeStringArray[i],out Time[i]);
			}

			Reward = tables[2];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, OnlineAwardConfig> configs = new Dictionary<int, OnlineAwardConfig>();
    public static OnlineAwardConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        OnlineAwardConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new OnlineAwardConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "OnlineAward.txt";
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

			DebugEx.LogFormat("加载结束OnlineAwardConfig：{0}",   DateTime.Now);
        });
    }

}




