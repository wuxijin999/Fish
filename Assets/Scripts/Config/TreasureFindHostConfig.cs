//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TreasureFindHostConfig
{

    public readonly int ID;
	public readonly int Type;
	public readonly int NeedCnt;
	public readonly string Condition;
	public readonly int MagicWeaponID;
	public readonly string AwardItemList;
	public readonly string Money;
	public readonly string AdviceIds;
	public readonly int JumpID;
	public readonly string[] EffectIconKeys;

    public TreasureFindHostConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out Type); 

			int.TryParse(tables[2],out NeedCnt); 

			Condition = tables[3];

			int.TryParse(tables[4],out MagicWeaponID); 

			AwardItemList = tables[5];

			Money = tables[6];

			AdviceIds = tables[7];

			int.TryParse(tables[8],out JumpID); 

			EffectIconKeys = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TreasureFindHostConfig> configs = new Dictionary<int, TreasureFindHostConfig>();
    public static TreasureFindHostConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TreasureFindHostConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TreasureFindHostConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TreasureFindHost.txt";
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

			DebugEx.LogFormat("加载结束TreasureFindHostConfig：{0}",   DateTime.Now);
        });
    }

}




