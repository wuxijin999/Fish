//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DungeonStateTimeConfig
{

    public readonly int ID;
	public readonly int DataMapID;
	public readonly int OpenServerWeek;
	public readonly int OpenServerDay;
	public readonly int StartWeekday;
	public readonly int StartHour;
	public readonly int StartMinute;
	public readonly int EndWeekday;
	public readonly int EndHour;
	public readonly int EndMinute;
	public readonly int CanEnter;
	public readonly int StateValue;

    public DungeonStateTimeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out DataMapID); 

			int.TryParse(tables[2],out OpenServerWeek); 

			int.TryParse(tables[3],out OpenServerDay); 

			int.TryParse(tables[4],out StartWeekday); 

			int.TryParse(tables[5],out StartHour); 

			int.TryParse(tables[6],out StartMinute); 

			int.TryParse(tables[7],out EndWeekday); 

			int.TryParse(tables[8],out EndHour); 

			int.TryParse(tables[9],out EndMinute); 

			int.TryParse(tables[10],out CanEnter); 

			int.TryParse(tables[11],out StateValue); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DungeonStateTimeConfig> configs = new Dictionary<int, DungeonStateTimeConfig>();
    public static DungeonStateTimeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DungeonStateTimeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DungeonStateTimeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DungeonStateTime.txt";
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

			DebugEx.LogFormat("加载结束DungeonStateTimeConfig：{0}",   DateTime.Now);
        });
    }

}




