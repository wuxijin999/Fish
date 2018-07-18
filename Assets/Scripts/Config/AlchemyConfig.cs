//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class AlchemyConfig
{

    public readonly int AlchemyID;
	public readonly string AlchemName;
	public readonly string MaterialAll;
	public readonly int AlchemyEXP;
	public readonly string AlchemyItem;
	public readonly int[] SpecialItem;
	public readonly string AlchemyIUp;
	public readonly string AlchemPreviewItem;
	public readonly int BlastFurnaceLV;
	public readonly string ICONID;

    public AlchemyConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out AlchemyID); 

			AlchemName = tables[1];

			MaterialAll = tables[2];

			int.TryParse(tables[3],out AlchemyEXP); 

			AlchemyItem = tables[4];

			string[] SpecialItemStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			SpecialItem = new int[SpecialItemStringArray.Length];
			for (int i=0;i<SpecialItemStringArray.Length;i++)
			{
				 int.TryParse(SpecialItemStringArray[i],out SpecialItem[i]);
			}

			AlchemyIUp = tables[6];

			AlchemPreviewItem = tables[7];

			int.TryParse(tables[8],out BlastFurnaceLV); 

			ICONID = tables[9];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, AlchemyConfig> configs = new Dictionary<int, AlchemyConfig>();
    public static AlchemyConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        AlchemyConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new AlchemyConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Alchemy.txt";
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

			DebugEx.LogFormat("加载结束AlchemyConfig：{0}",   DateTime.Now);
        });
    }

}




