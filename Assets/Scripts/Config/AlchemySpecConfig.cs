//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class AlchemySpecConfig
{

    public readonly int ID;
	public readonly int SpecialMaterialD;
	public readonly int SpecialMateriaNUM;
	public readonly int AlchemyEXP;
	public readonly string AlchemyItem;
	public readonly string AlchemPreviewItem;
	public readonly int BlastFurnaceLV;
	public readonly string Introduce;

    public AlchemySpecConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out SpecialMaterialD); 

			int.TryParse(tables[2],out SpecialMateriaNUM); 

			int.TryParse(tables[3],out AlchemyEXP); 

			AlchemyItem = tables[4];

			AlchemPreviewItem = tables[5];

			int.TryParse(tables[6],out BlastFurnaceLV); 

			Introduce = tables[7];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, AlchemySpecConfig> configs = new Dictionary<int, AlchemySpecConfig>();
    public static AlchemySpecConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        AlchemySpecConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new AlchemySpecConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "AlchemySpec.txt";
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

			DebugEx.LogFormat("加载结束AlchemySpecConfig：{0}",   DateTime.Now);
        });
    }

}




