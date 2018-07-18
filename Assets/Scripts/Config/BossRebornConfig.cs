//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class BossRebornConfig
{

    public readonly int Id;
	public readonly int TotalTimes;
	public readonly int SingleTimes;
	public readonly int[] Reward1;
	public readonly int[] RewardCount1;
	public readonly int[] Reward2;
	public readonly int[] RewardCount2;
	public readonly int[] Reward3;
	public readonly int[] RewardCount3;
	public readonly int[] WorldLevel;
	public readonly string Description;
	public readonly int jump;

    public BossRebornConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out Id); 

			int.TryParse(tables[1],out TotalTimes); 

			int.TryParse(tables[2],out SingleTimes); 

			string[] Reward1StringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Reward1 = new int[Reward1StringArray.Length];
			for (int i=0;i<Reward1StringArray.Length;i++)
			{
				 int.TryParse(Reward1StringArray[i],out Reward1[i]);
			}

			string[] RewardCount1StringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RewardCount1 = new int[RewardCount1StringArray.Length];
			for (int i=0;i<RewardCount1StringArray.Length;i++)
			{
				 int.TryParse(RewardCount1StringArray[i],out RewardCount1[i]);
			}

			string[] Reward2StringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Reward2 = new int[Reward2StringArray.Length];
			for (int i=0;i<Reward2StringArray.Length;i++)
			{
				 int.TryParse(Reward2StringArray[i],out Reward2[i]);
			}

			string[] RewardCount2StringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RewardCount2 = new int[RewardCount2StringArray.Length];
			for (int i=0;i<RewardCount2StringArray.Length;i++)
			{
				 int.TryParse(RewardCount2StringArray[i],out RewardCount2[i]);
			}

			string[] Reward3StringArray = tables[7].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Reward3 = new int[Reward3StringArray.Length];
			for (int i=0;i<Reward3StringArray.Length;i++)
			{
				 int.TryParse(Reward3StringArray[i],out Reward3[i]);
			}

			string[] RewardCount3StringArray = tables[8].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RewardCount3 = new int[RewardCount3StringArray.Length];
			for (int i=0;i<RewardCount3StringArray.Length;i++)
			{
				 int.TryParse(RewardCount3StringArray[i],out RewardCount3[i]);
			}

			string[] WorldLevelStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			WorldLevel = new int[WorldLevelStringArray.Length];
			for (int i=0;i<WorldLevelStringArray.Length;i++)
			{
				 int.TryParse(WorldLevelStringArray[i],out WorldLevel[i]);
			}

			Description = tables[10];

			int.TryParse(tables[11],out jump); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, BossRebornConfig> configs = new Dictionary<int, BossRebornConfig>();
    public static BossRebornConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        BossRebornConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new BossRebornConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "BossReborn.txt";
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

			DebugEx.LogFormat("加载结束BossRebornConfig：{0}",   DateTime.Now);
        });
    }

}




