//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class mapbornConfig
{

    public readonly int BornID;
	public readonly int MapID;
	public readonly int PosX;
	public readonly int PosY;
	public readonly int BornDiameter;
	public readonly int Nationality;

    public mapbornConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out BornID); 

			int.TryParse(tables[1],out MapID); 

			int.TryParse(tables[2],out PosX); 

			int.TryParse(tables[3],out PosY); 

			int.TryParse(tables[4],out BornDiameter); 

			int.TryParse(tables[5],out Nationality); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, mapbornConfig> configs = new Dictionary<int, mapbornConfig>();
    public static mapbornConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        mapbornConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new mapbornConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "mapborn.txt";
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

			DebugEx.LogFormat("加载结束mapbornConfig：{0}",   DateTime.Now);
        });
    }

}




