//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class MapConfig
{

    public readonly int MapID;
	public readonly string Name;
	public readonly int LV;
	public readonly int MapFBType;
	public readonly int LocalReborn;
	public readonly int SkillReborn;
	public readonly int CanRide;
	public readonly int CanOutPet;
	public readonly int TeamLimit;
	public readonly Vector3[] BornPoints;
	public readonly int MainTaskID;
	public readonly string MapTaskText;
	public readonly int Camp;
	public readonly int AtkType;

    public MapConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out MapID); 

			Name = tables[1];

			int.TryParse(tables[2],out LV); 

			int.TryParse(tables[3],out MapFBType); 

			int.TryParse(tables[4],out LocalReborn); 

			int.TryParse(tables[5],out SkillReborn); 

			int.TryParse(tables[6],out CanRide); 

			int.TryParse(tables[7],out CanOutPet); 

			int.TryParse(tables[8],out TeamLimit); 

			string[] BornPointsStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			BornPoints = new Vector3[BornPointsStringArray.Length];
			for (int i=0;i<BornPointsStringArray.Length;i++)
			{
				BornPoints[i]=BornPointsStringArray[i].Vector3Parse();
			}

			int.TryParse(tables[10],out MainTaskID); 

			MapTaskText = tables[11];

			int.TryParse(tables[12],out Camp); 

			int.TryParse(tables[13],out AtkType); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, MapConfig> configs = new Dictionary<int, MapConfig>();
    public static MapConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        MapConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new MapConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Map.txt";
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

			DebugEx.LogFormat("加载结束MapConfig：{0}",   DateTime.Now);
        });
    }

}




