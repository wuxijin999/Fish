//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DailyQuestConfig
{

    public readonly int ID;
	public readonly string Title;
	public readonly int RelatedType;
	public readonly int RelatedID;
	public readonly int UnLockFuncID;
	public readonly int OnceActivityTime;
	public readonly int OnceActivity;
	public readonly int TotalActiveValue;
	public readonly int[] RewardID;
	public readonly string Icon;
	public readonly string Description;
	public readonly string QuestTypeDescribe;
	public readonly int order;

    public DailyQuestConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			Title = tables[1];

			int.TryParse(tables[2],out RelatedType); 

			int.TryParse(tables[3],out RelatedID); 

			int.TryParse(tables[4],out UnLockFuncID); 

			int.TryParse(tables[5],out OnceActivityTime); 

			int.TryParse(tables[6],out OnceActivity); 

			int.TryParse(tables[7],out TotalActiveValue); 

			string[] RewardIDStringArray = tables[8].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RewardID = new int[RewardIDStringArray.Length];
			for (int i=0;i<RewardIDStringArray.Length;i++)
			{
				 int.TryParse(RewardIDStringArray[i],out RewardID[i]);
			}

			Icon = tables[9];

			Description = tables[10];

			QuestTypeDescribe = tables[11];

			int.TryParse(tables[12],out order); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DailyQuestConfig> configs = new Dictionary<int, DailyQuestConfig>();
    public static DailyQuestConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DailyQuestConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DailyQuestConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DailyQuest.txt";
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

			DebugEx.LogFormat("加载结束DailyQuestConfig：{0}",   DateTime.Now);
        });
    }

}




