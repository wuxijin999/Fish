//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DungeonOpenTimeConfig
{

    public readonly int DataMapID;
	public readonly string FBName;
	public readonly int OpenServerDay;
	public readonly int CanEnterTime;
	public readonly int DayTimes;
	public readonly int DayReKind;
	public readonly int WeekTimes;
	public readonly int WeekReKind;
	public readonly int FBType;
	public readonly int[] RewardRate;
	public readonly int BuyTimesID;
	public readonly int ExtraTimesID;
	public readonly int DeathTime;
	public readonly int GuardPick;
	public readonly int DoFight;
	public readonly int HelpPoint;
	public readonly int DelayTime;
	public readonly int Movable;
	public readonly int Skillable;
	public readonly int SelectPlayerable;
	public readonly string ExitDescription;
	public readonly string PanelImg;
	public readonly int[] ElixirHint;

    public DungeonOpenTimeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out DataMapID); 

			FBName = tables[1];

			int.TryParse(tables[2],out OpenServerDay); 

			int.TryParse(tables[3],out CanEnterTime); 

			int.TryParse(tables[4],out DayTimes); 

			int.TryParse(tables[5],out DayReKind); 

			int.TryParse(tables[6],out WeekTimes); 

			int.TryParse(tables[7],out WeekReKind); 

			int.TryParse(tables[8],out FBType); 

			string[] RewardRateStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RewardRate = new int[RewardRateStringArray.Length];
			for (int i=0;i<RewardRateStringArray.Length;i++)
			{
				 int.TryParse(RewardRateStringArray[i],out RewardRate[i]);
			}

			int.TryParse(tables[10],out BuyTimesID); 

			int.TryParse(tables[11],out ExtraTimesID); 

			int.TryParse(tables[12],out DeathTime); 

			int.TryParse(tables[13],out GuardPick); 

			int.TryParse(tables[14],out DoFight); 

			int.TryParse(tables[15],out HelpPoint); 

			int.TryParse(tables[16],out DelayTime); 

			int.TryParse(tables[17],out Movable); 

			int.TryParse(tables[18],out Skillable); 

			int.TryParse(tables[19],out SelectPlayerable); 

			ExitDescription = tables[20];

			PanelImg = tables[21];

			string[] ElixirHintStringArray = tables[22].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ElixirHint = new int[ElixirHintStringArray.Length];
			for (int i=0;i<ElixirHintStringArray.Length;i++)
			{
				 int.TryParse(ElixirHintStringArray[i],out ElixirHint[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DungeonOpenTimeConfig> configs = new Dictionary<int, DungeonOpenTimeConfig>();
    public static DungeonOpenTimeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DungeonOpenTimeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DungeonOpenTimeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DungeonOpenTime.txt";
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

			DebugEx.LogFormat("加载结束DungeonOpenTimeConfig：{0}",   DateTime.Now);
        });
    }

}




