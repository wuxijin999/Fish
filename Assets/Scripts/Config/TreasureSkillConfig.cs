//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TreasureSkillConfig
{

    public readonly int ID;
	public readonly int SkillType;
	public readonly int SkillLv;
	public readonly int MeterialNum1;
	public readonly int InitialRate;
	public readonly int[] Meterial2ID;
	public readonly int[] MeterialNum2;
	public readonly int[] Rate;

    public TreasureSkillConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out SkillType); 

			int.TryParse(tables[2],out SkillLv); 

			int.TryParse(tables[3],out MeterialNum1); 

			int.TryParse(tables[4],out InitialRate); 

			string[] Meterial2IDStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Meterial2ID = new int[Meterial2IDStringArray.Length];
			for (int i=0;i<Meterial2IDStringArray.Length;i++)
			{
				 int.TryParse(Meterial2IDStringArray[i],out Meterial2ID[i]);
			}

			string[] MeterialNum2StringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			MeterialNum2 = new int[MeterialNum2StringArray.Length];
			for (int i=0;i<MeterialNum2StringArray.Length;i++)
			{
				 int.TryParse(MeterialNum2StringArray[i],out MeterialNum2[i]);
			}

			string[] RateStringArray = tables[7].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Rate = new int[RateStringArray.Length];
			for (int i=0;i<RateStringArray.Length;i++)
			{
				 int.TryParse(RateStringArray[i],out Rate[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TreasureSkillConfig> configs = new Dictionary<int, TreasureSkillConfig>();
    public static TreasureSkillConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TreasureSkillConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TreasureSkillConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TreasureSkill.txt";
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

			DebugEx.LogFormat("加载结束TreasureSkillConfig：{0}",   DateTime.Now);
        });
    }

}




