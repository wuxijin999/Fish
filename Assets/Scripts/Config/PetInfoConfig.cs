//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PetInfoConfig
{

    public readonly int ID;
	public readonly string Name;
	public readonly int Quality;
	public readonly int UnLockNeedItemID;
	public readonly int UnLockNeedItemCnt;
	public readonly int DecomposeExp;
	public readonly int InitRank;
	public readonly int MaxRank;
	public readonly int UseNeedRank;
	public readonly string SkillID;
	public readonly string SkillScore;
	public readonly string SkillUnLock;
	public readonly int[] ShowSkill;
	public readonly string IconKey;
	public readonly string InitFightPower;
	public readonly int ShowFightPower;
	public readonly int Sort;

    public PetInfoConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			Name = tables[1];

			int.TryParse(tables[2],out Quality); 

			int.TryParse(tables[3],out UnLockNeedItemID); 

			int.TryParse(tables[4],out UnLockNeedItemCnt); 

			int.TryParse(tables[5],out DecomposeExp); 

			int.TryParse(tables[6],out InitRank); 

			int.TryParse(tables[7],out MaxRank); 

			int.TryParse(tables[8],out UseNeedRank); 

			SkillID = tables[9];

			SkillScore = tables[10];

			SkillUnLock = tables[11];

			string[] ShowSkillStringArray = tables[12].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ShowSkill = new int[ShowSkillStringArray.Length];
			for (int i=0;i<ShowSkillStringArray.Length;i++)
			{
				 int.TryParse(ShowSkillStringArray[i],out ShowSkill[i]);
			}

			IconKey = tables[13];

			InitFightPower = tables[14];

			int.TryParse(tables[15],out ShowFightPower); 

			int.TryParse(tables[16],out Sort); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PetInfoConfig> configs = new Dictionary<int, PetInfoConfig>();
    public static PetInfoConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        PetInfoConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new PetInfoConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "PetInfo.txt";
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

			DebugEx.LogFormat("加载结束PetInfoConfig：{0}",   DateTime.Now);
        });
    }

}




