//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DailyLivenessRewardConfig
{

    public readonly int id;
	public readonly int Liveness;
	public readonly int[] StageLV;
	public readonly int[] ItemID;
	public readonly int[] ItemCount;
	public readonly int[] ItemBind;
	public readonly string Description;

    public DailyLivenessRewardConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out Liveness); 

			string[] StageLVStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			StageLV = new int[StageLVStringArray.Length];
			for (int i=0;i<StageLVStringArray.Length;i++)
			{
				 int.TryParse(StageLVStringArray[i],out StageLV[i]);
			}

			string[] ItemIDStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ItemID = new int[ItemIDStringArray.Length];
			for (int i=0;i<ItemIDStringArray.Length;i++)
			{
				 int.TryParse(ItemIDStringArray[i],out ItemID[i]);
			}

			string[] ItemCountStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ItemCount = new int[ItemCountStringArray.Length];
			for (int i=0;i<ItemCountStringArray.Length;i++)
			{
				 int.TryParse(ItemCountStringArray[i],out ItemCount[i]);
			}

			string[] ItemBindStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ItemBind = new int[ItemBindStringArray.Length];
			for (int i=0;i<ItemBindStringArray.Length;i++)
			{
				 int.TryParse(ItemBindStringArray[i],out ItemBind[i]);
			}

			Description = tables[6];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DailyLivenessRewardConfig> configs = new Dictionary<int, DailyLivenessRewardConfig>();
    public static DailyLivenessRewardConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DailyLivenessRewardConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DailyLivenessRewardConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DailyLivenessReward.txt";
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

			DebugEx.LogFormat("加载结束DailyLivenessRewardConfig：{0}",   DateTime.Now);
        });
    }

}




