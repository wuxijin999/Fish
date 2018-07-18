//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class FamilyConfig
{

    public readonly int familyLV;
	public readonly int memberCnt;
	public readonly int deputyLeaderCnt;
	public readonly int eliteCnt;
	public readonly int needMoney;
	public readonly int weekMissionMoneyMax;

    public FamilyConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out familyLV); 

			int.TryParse(tables[1],out memberCnt); 

			int.TryParse(tables[2],out deputyLeaderCnt); 

			int.TryParse(tables[3],out eliteCnt); 

			int.TryParse(tables[4],out needMoney); 

			int.TryParse(tables[5],out weekMissionMoneyMax); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, FamilyConfig> configs = new Dictionary<int, FamilyConfig>();
    public static FamilyConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        FamilyConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new FamilyConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Family.txt";
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

			DebugEx.LogFormat("加载结束FamilyConfig：{0}",   DateTime.Now);
        });
    }

}




