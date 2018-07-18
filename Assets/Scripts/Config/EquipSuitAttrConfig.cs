//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EquipSuitAttrConfig
{

    public readonly int id;
	public readonly string name;
	public readonly int groupType;
	public readonly int suiteType;
	public readonly int suiteLV;
	public readonly int job;
	public readonly int count1;
	public readonly string propList1;
	public readonly string propValueList1;
	public readonly int count2;
	public readonly string propList2;
	public readonly string propValueList2;
	public readonly int count3;
	public readonly string propList3;
	public readonly string propValueList3;
	public readonly string[] bindbones;
	public readonly int[] effectIds;

    public EquipSuitAttrConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			name = tables[1];

			int.TryParse(tables[2],out groupType); 

			int.TryParse(tables[3],out suiteType); 

			int.TryParse(tables[4],out suiteLV); 

			int.TryParse(tables[5],out job); 

			int.TryParse(tables[6],out count1); 

			propList1 = tables[7];

			propValueList1 = tables[8];

			int.TryParse(tables[9],out count2); 

			propList2 = tables[10];

			propValueList2 = tables[11];

			int.TryParse(tables[12],out count3); 

			propList3 = tables[13];

			propValueList3 = tables[14];

			bindbones = tables[15].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			string[] effectIdsStringArray = tables[16].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			effectIds = new int[effectIdsStringArray.Length];
			for (int i=0;i<effectIdsStringArray.Length;i++)
			{
				 int.TryParse(effectIdsStringArray[i],out effectIds[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EquipSuitAttrConfig> configs = new Dictionary<int, EquipSuitAttrConfig>();
    public static EquipSuitAttrConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        EquipSuitAttrConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new EquipSuitAttrConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "EquipSuitAttr.txt";
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

			DebugEx.LogFormat("加载结束EquipSuitAttrConfig：{0}",   DateTime.Now);
        });
    }

}




