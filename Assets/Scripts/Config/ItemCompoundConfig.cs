//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ItemCompoundConfig
{

    public readonly int id;
	public readonly int firstType;
	public readonly int levelNeed;
	public readonly int secondType;
	public readonly string secondTypeName;
	public readonly int thirdType;
	public readonly string thirdTypeName;
	public readonly string makeID;
	public readonly string unfixedItemID;
	public readonly int unfixedItemCount;
	public readonly string unfixedItemDisplay;
	public readonly string itemID;
	public readonly string itemCount;
	public readonly string itemDisplay;
	public readonly int money;
	public readonly int successRate;
	public readonly int successUpper;
	public readonly int addonsCountMax;
	public readonly string helpDesc;

    public ItemCompoundConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out firstType); 

			int.TryParse(tables[2],out levelNeed); 

			int.TryParse(tables[3],out secondType); 

			secondTypeName = tables[4];

			int.TryParse(tables[5],out thirdType); 

			thirdTypeName = tables[6];

			makeID = tables[7];

			unfixedItemID = tables[8];

			int.TryParse(tables[9],out unfixedItemCount); 

			unfixedItemDisplay = tables[10];

			itemID = tables[11];

			itemCount = tables[12];

			itemDisplay = tables[13];

			int.TryParse(tables[14],out money); 

			int.TryParse(tables[15],out successRate); 

			int.TryParse(tables[16],out successUpper); 

			int.TryParse(tables[17],out addonsCountMax); 

			helpDesc = tables[18];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ItemCompoundConfig> configs = new Dictionary<int, ItemCompoundConfig>();
    public static ItemCompoundConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ItemCompoundConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ItemCompoundConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ItemCompound.txt";
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

			DebugEx.LogFormat("加载结束ItemCompoundConfig：{0}",   DateTime.Now);
        });
    }

}




