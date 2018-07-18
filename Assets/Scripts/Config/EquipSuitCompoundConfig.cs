//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EquipSuitCompoundConfig
{

    public readonly int ID;
	public readonly int SuiteType;
	public readonly int EquipPlace;
	public readonly int SuiteLV;
	public readonly int Job;
	public readonly int[] CostItemID;
	public readonly int[] CostItemCnt;

    public EquipSuitCompoundConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out SuiteType); 

			int.TryParse(tables[2],out EquipPlace); 

			int.TryParse(tables[3],out SuiteLV); 

			int.TryParse(tables[4],out Job); 

			string[] CostItemIDStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			CostItemID = new int[CostItemIDStringArray.Length];
			for (int i=0;i<CostItemIDStringArray.Length;i++)
			{
				 int.TryParse(CostItemIDStringArray[i],out CostItemID[i]);
			}

			string[] CostItemCntStringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			CostItemCnt = new int[CostItemCntStringArray.Length];
			for (int i=0;i<CostItemCntStringArray.Length;i++)
			{
				 int.TryParse(CostItemCntStringArray[i],out CostItemCnt[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EquipSuitCompoundConfig> configs = new Dictionary<int, EquipSuitCompoundConfig>();
    public static EquipSuitCompoundConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        EquipSuitCompoundConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new EquipSuitCompoundConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "EquipSuitCompound.txt";
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

			DebugEx.LogFormat("加载结束EquipSuitCompoundConfig：{0}",   DateTime.Now);
        });
    }

}




