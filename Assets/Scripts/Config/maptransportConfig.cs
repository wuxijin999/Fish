//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class maptransportConfig
{

    public readonly int TransportID;
	public readonly int OriginalMapID;
	public readonly int OriginalPosX;
	public readonly int OriginalPosY;
	public readonly int TransportDiameter;
	public readonly int TargetMapID;
	public readonly int PerformID;
	public readonly int TransportType;
	public readonly int OutPosX;
	public readonly int OutPosY;

    public maptransportConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out TransportID); 

			int.TryParse(tables[1],out OriginalMapID); 

			int.TryParse(tables[2],out OriginalPosX); 

			int.TryParse(tables[3],out OriginalPosY); 

			int.TryParse(tables[4],out TransportDiameter); 

			int.TryParse(tables[5],out TargetMapID); 

			int.TryParse(tables[6],out PerformID); 

			int.TryParse(tables[7],out TransportType); 

			int.TryParse(tables[8],out OutPosX); 

			int.TryParse(tables[9],out OutPosY); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, maptransportConfig> configs = new Dictionary<int, maptransportConfig>();
    public static maptransportConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        maptransportConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new maptransportConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "maptransport.txt";
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

			DebugEx.LogFormat("加载结束maptransportConfig：{0}",   DateTime.Now);
        });
    }

}




