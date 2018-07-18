//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DungeonConfig
{

    public readonly int ID;
	public readonly int DataMapID;
	public readonly int LineID;
	public readonly int MapID;
	public readonly string FBName;
	public readonly int LVLimitMin;
	public readonly int LVLimitMax;
	public readonly int JobRankLimit;
	public readonly int TicketID;
	public readonly int[] TicketCostCnt;
	public readonly int TicketPrice;
	public readonly int SweepLVLimit;
	public readonly int SweepItemID;
	public readonly int SweepCostCnt;
	public readonly string StepTime;
	public readonly int[] BossActorID;
	public readonly int[] Rewards;
	public readonly string Description;
	public readonly int AutomaticATK;
	public readonly int MapButton;
	public readonly int ShowNewItemTip;

    public DungeonConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out DataMapID); 

			int.TryParse(tables[2],out LineID); 

			int.TryParse(tables[3],out MapID); 

			FBName = tables[4];

			int.TryParse(tables[5],out LVLimitMin); 

			int.TryParse(tables[6],out LVLimitMax); 

			int.TryParse(tables[7],out JobRankLimit); 

			int.TryParse(tables[8],out TicketID); 

			string[] TicketCostCntStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			TicketCostCnt = new int[TicketCostCntStringArray.Length];
			for (int i=0;i<TicketCostCntStringArray.Length;i++)
			{
				 int.TryParse(TicketCostCntStringArray[i],out TicketCostCnt[i]);
			}

			int.TryParse(tables[10],out TicketPrice); 

			int.TryParse(tables[11],out SweepLVLimit); 

			int.TryParse(tables[12],out SweepItemID); 

			int.TryParse(tables[13],out SweepCostCnt); 

			StepTime = tables[14];

			string[] BossActorIDStringArray = tables[15].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			BossActorID = new int[BossActorIDStringArray.Length];
			for (int i=0;i<BossActorIDStringArray.Length;i++)
			{
				 int.TryParse(BossActorIDStringArray[i],out BossActorID[i]);
			}

			string[] RewardsStringArray = tables[16].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Rewards = new int[RewardsStringArray.Length];
			for (int i=0;i<RewardsStringArray.Length;i++)
			{
				 int.TryParse(RewardsStringArray[i],out Rewards[i]);
			}

			Description = tables[17];

			int.TryParse(tables[18],out AutomaticATK); 

			int.TryParse(tables[19],out MapButton); 

			int.TryParse(tables[20],out ShowNewItemTip); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DungeonConfig> configs = new Dictionary<int, DungeonConfig>();
    public static DungeonConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DungeonConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DungeonConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Dungeon.txt";
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

			DebugEx.LogFormat("加载结束DungeonConfig：{0}",   DateTime.Now);
        });
    }

}




