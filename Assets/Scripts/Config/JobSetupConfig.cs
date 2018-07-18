//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class JobSetupConfig
{

    public readonly int Job;
	public readonly int[] BaseEquip;
	public readonly int[] ComAtkIdList;
	public readonly int[] StopAction;
	public readonly int[] CanStopSkillList;
	public readonly string CanBeStopSkillList;
	public readonly int RushMinDist;
	public readonly int RushMaxDist;
	public readonly int RushSpeed;
	public readonly int RushInterval;
	public readonly int RushAnimatorSpeed;
	public readonly int RushTargetType;
	public readonly int ShadowLastTime;
	public readonly int ShadowCreateInterval;
	public readonly int SearchEnemyDist;
	public readonly int MaxSwitchTargetDist;
	public readonly int[] HangupSkillList;
	public readonly string DungeonSkillList;
	public readonly int HpPerUseSkill;
	public readonly int[] HpSkillList;
	public readonly int[] GainSkillList;
	public readonly int MoveLimitDist;

    public JobSetupConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out Job); 

			string[] BaseEquipStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			BaseEquip = new int[BaseEquipStringArray.Length];
			for (int i=0;i<BaseEquipStringArray.Length;i++)
			{
				 int.TryParse(BaseEquipStringArray[i],out BaseEquip[i]);
			}

			string[] ComAtkIdListStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ComAtkIdList = new int[ComAtkIdListStringArray.Length];
			for (int i=0;i<ComAtkIdListStringArray.Length;i++)
			{
				 int.TryParse(ComAtkIdListStringArray[i],out ComAtkIdList[i]);
			}

			string[] StopActionStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			StopAction = new int[StopActionStringArray.Length];
			for (int i=0;i<StopActionStringArray.Length;i++)
			{
				 int.TryParse(StopActionStringArray[i],out StopAction[i]);
			}

			string[] CanStopSkillListStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			CanStopSkillList = new int[CanStopSkillListStringArray.Length];
			for (int i=0;i<CanStopSkillListStringArray.Length;i++)
			{
				 int.TryParse(CanStopSkillListStringArray[i],out CanStopSkillList[i]);
			}

			CanBeStopSkillList = tables[5];

			int.TryParse(tables[6],out RushMinDist); 

			int.TryParse(tables[7],out RushMaxDist); 

			int.TryParse(tables[8],out RushSpeed); 

			int.TryParse(tables[9],out RushInterval); 

			int.TryParse(tables[10],out RushAnimatorSpeed); 

			int.TryParse(tables[11],out RushTargetType); 

			int.TryParse(tables[12],out ShadowLastTime); 

			int.TryParse(tables[13],out ShadowCreateInterval); 

			int.TryParse(tables[14],out SearchEnemyDist); 

			int.TryParse(tables[15],out MaxSwitchTargetDist); 

			string[] HangupSkillListStringArray = tables[16].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			HangupSkillList = new int[HangupSkillListStringArray.Length];
			for (int i=0;i<HangupSkillListStringArray.Length;i++)
			{
				 int.TryParse(HangupSkillListStringArray[i],out HangupSkillList[i]);
			}

			DungeonSkillList = tables[17];

			int.TryParse(tables[18],out HpPerUseSkill); 

			string[] HpSkillListStringArray = tables[19].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			HpSkillList = new int[HpSkillListStringArray.Length];
			for (int i=0;i<HpSkillListStringArray.Length;i++)
			{
				 int.TryParse(HpSkillListStringArray[i],out HpSkillList[i]);
			}

			string[] GainSkillListStringArray = tables[20].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			GainSkillList = new int[GainSkillListStringArray.Length];
			for (int i=0;i<GainSkillListStringArray.Length;i++)
			{
				 int.TryParse(GainSkillListStringArray[i],out GainSkillList[i]);
			}

			int.TryParse(tables[21],out MoveLimitDist); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, JobSetupConfig> configs = new Dictionary<int, JobSetupConfig>();
    public static JobSetupConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        JobSetupConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new JobSetupConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "JobSetup.txt";
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

			DebugEx.LogFormat("加载结束JobSetupConfig：{0}",   DateTime.Now);
        });
    }

}




