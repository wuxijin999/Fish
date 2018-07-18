//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EquipWashConfig
{

    public readonly int id;
	public readonly int type;
	public readonly int level;
	public readonly int attType1;
	public readonly int attMax1;
	public readonly int attCostMoneyMin1;
	public readonly int attCostMoneyMax1;
	public readonly int attType2;
	public readonly int attMax2;
	public readonly int attCostMoneyMin2;
	public readonly int attCostMoneyMax2;
	public readonly int attType3;
	public readonly int attMax3;
	public readonly int attCostMoneyMin3;
	public readonly int attCostMoneyMax3;
	public readonly int costItem;
	public readonly int costCount;
	public readonly string costMoneyList;

    public EquipWashConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out type); 

			int.TryParse(tables[2],out level); 

			int.TryParse(tables[3],out attType1); 

			int.TryParse(tables[4],out attMax1); 

			int.TryParse(tables[5],out attCostMoneyMin1); 

			int.TryParse(tables[6],out attCostMoneyMax1); 

			int.TryParse(tables[7],out attType2); 

			int.TryParse(tables[8],out attMax2); 

			int.TryParse(tables[9],out attCostMoneyMin2); 

			int.TryParse(tables[10],out attCostMoneyMax2); 

			int.TryParse(tables[11],out attType3); 

			int.TryParse(tables[12],out attMax3); 

			int.TryParse(tables[13],out attCostMoneyMin3); 

			int.TryParse(tables[14],out attCostMoneyMax3); 

			int.TryParse(tables[15],out costItem); 

			int.TryParse(tables[16],out costCount); 

			costMoneyList = tables[17];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EquipWashConfig> configs = new Dictionary<int, EquipWashConfig>();
    public static EquipWashConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        EquipWashConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new EquipWashConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "EquipWash.txt";
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

			DebugEx.LogFormat("加载结束EquipWashConfig：{0}",   DateTime.Now);
        });
    }

}




