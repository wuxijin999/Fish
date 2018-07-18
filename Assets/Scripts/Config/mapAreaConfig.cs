//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class mapAreaConfig
{

    public readonly int AreaID;
	public readonly int MapID;
	public readonly int StartPosX;
	public readonly int StartPosY;
	public readonly int EndPosX;
	public readonly int EndPosY;
	public readonly int AreaName;

    public mapAreaConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out AreaID); 

			int.TryParse(tables[1],out MapID); 

			int.TryParse(tables[2],out StartPosX); 

			int.TryParse(tables[3],out StartPosY); 

			int.TryParse(tables[4],out EndPosX); 

			int.TryParse(tables[5],out EndPosY); 

			int.TryParse(tables[6],out AreaName); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, mapAreaConfig> configs = new Dictionary<int, mapAreaConfig>();
    public static mapAreaConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        mapAreaConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new mapAreaConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "mapArea.txt";
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

			DebugEx.LogFormat("加载结束mapAreaConfig：{0}",   DateTime.Now);
        });
    }

}




