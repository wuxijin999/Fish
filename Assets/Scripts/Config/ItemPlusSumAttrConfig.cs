//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ItemPlusSumAttrConfig
{

    public readonly int countNeed;
	public readonly int[] attType;
	public readonly int[] attValue;

    public ItemPlusSumAttrConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out countNeed); 

			string[] attTypeStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			attType = new int[attTypeStringArray.Length];
			for (int i=0;i<attTypeStringArray.Length;i++)
			{
				 int.TryParse(attTypeStringArray[i],out attType[i]);
			}

			string[] attValueStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			attValue = new int[attValueStringArray.Length];
			for (int i=0;i<attValueStringArray.Length;i++)
			{
				 int.TryParse(attValueStringArray[i],out attValue[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ItemPlusSumAttrConfig> configs = new Dictionary<int, ItemPlusSumAttrConfig>();
    public static ItemPlusSumAttrConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ItemPlusSumAttrConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ItemPlusSumAttrConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ItemPlusSumAttr.txt";
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

			DebugEx.LogFormat("加载结束ItemPlusSumAttrConfig：{0}",   DateTime.Now);
        });
    }

}




