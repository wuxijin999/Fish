//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class MapEventPointConfig
{

    public readonly int Key;
	public readonly int MapID;
	public readonly int NPCID;
	public readonly int IsShowInfo;
	public readonly int ShowInMipMap;
	public readonly int Colour;
	public readonly int LowLV;
	public readonly int HighestLV;
	public readonly int Defense;
	public readonly string Drop1;
	public readonly string Drop2;
	public readonly string EXP;

    public MapEventPointConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out Key); 

			int.TryParse(tables[1],out MapID); 

			int.TryParse(tables[2],out NPCID); 

			int.TryParse(tables[3],out IsShowInfo); 

			int.TryParse(tables[4],out ShowInMipMap); 

			int.TryParse(tables[5],out Colour); 

			int.TryParse(tables[6],out LowLV); 

			int.TryParse(tables[7],out HighestLV); 

			int.TryParse(tables[8],out Defense); 

			Drop1 = tables[9];

			Drop2 = tables[10];

			EXP = tables[11];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, MapEventPointConfig> configs = new Dictionary<int, MapEventPointConfig>();
    public static MapEventPointConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        MapEventPointConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new MapEventPointConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "MapEventPoint.txt";
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

			DebugEx.LogFormat("加载结束MapEventPointConfig：{0}",   DateTime.Now);
        });
    }

}




