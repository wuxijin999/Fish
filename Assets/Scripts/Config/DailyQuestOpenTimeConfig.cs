//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DailyQuestOpenTimeConfig
{

    public readonly int ID;
	public readonly string ActionName;
	public readonly int OpenServerDay;
	public readonly string OpenTime;
	public readonly int Duration;
	public readonly int DayTimes;
	public readonly int DayReKind;
	public readonly int WeekTimes;
	public readonly int WeekReKind;
	public readonly int OpenUI;

    public DailyQuestOpenTimeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			ActionName = tables[1];

			int.TryParse(tables[2],out OpenServerDay); 

			OpenTime = tables[3];

			int.TryParse(tables[4],out Duration); 

			int.TryParse(tables[5],out DayTimes); 

			int.TryParse(tables[6],out DayReKind); 

			int.TryParse(tables[7],out WeekTimes); 

			int.TryParse(tables[8],out WeekReKind); 

			int.TryParse(tables[9],out OpenUI); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DailyQuestOpenTimeConfig> configs = new Dictionary<int, DailyQuestOpenTimeConfig>();
    public static DailyQuestOpenTimeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DailyQuestOpenTimeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DailyQuestOpenTimeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DailyQuestOpenTime.txt";
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

			DebugEx.LogFormat("加载结束DailyQuestOpenTimeConfig：{0}",   DateTime.Now);
        });
    }

}




