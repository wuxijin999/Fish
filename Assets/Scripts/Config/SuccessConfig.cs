//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class SuccessConfig
{

    public readonly int ID;
	public readonly int Type;
	public readonly int Group;
	public readonly int NeedCnt;
	public readonly int[] Condition;
	public readonly string Condition2;
	public readonly int Condition3;
	public readonly string AwardItemList;
	public readonly string Money;
	public readonly int Exp;
	public readonly int[] AwardAttribute;
	public readonly int RedPacketID;
	public readonly int MagicWeaponID;
	public readonly string MagicWeaponExp;
	public readonly string Describe;
	public readonly int NeedGoto;
	public readonly int Jump;
	public readonly int ReOrder;
	public readonly int RealmPracticeID;

    public SuccessConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out Type); 

			int.TryParse(tables[2],out Group); 

			int.TryParse(tables[3],out NeedCnt); 

			string[] ConditionStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Condition = new int[ConditionStringArray.Length];
			for (int i=0;i<ConditionStringArray.Length;i++)
			{
				 int.TryParse(ConditionStringArray[i],out Condition[i]);
			}

			Condition2 = tables[5];

			int.TryParse(tables[6],out Condition3); 

			AwardItemList = tables[7];

			Money = tables[8];

			int.TryParse(tables[9],out Exp); 

			string[] AwardAttributeStringArray = tables[10].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AwardAttribute = new int[AwardAttributeStringArray.Length];
			for (int i=0;i<AwardAttributeStringArray.Length;i++)
			{
				 int.TryParse(AwardAttributeStringArray[i],out AwardAttribute[i]);
			}

			int.TryParse(tables[11],out RedPacketID); 

			int.TryParse(tables[12],out MagicWeaponID); 

			MagicWeaponExp = tables[13];

			Describe = tables[14];

			int.TryParse(tables[15],out NeedGoto); 

			int.TryParse(tables[16],out Jump); 

			int.TryParse(tables[17],out ReOrder); 

			int.TryParse(tables[18],out RealmPracticeID); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, SuccessConfig> configs = new Dictionary<int, SuccessConfig>();
    public static SuccessConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        SuccessConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new SuccessConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Success.txt";
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

			DebugEx.LogFormat("加载结束SuccessConfig：{0}",   DateTime.Now);
        });
    }

}




