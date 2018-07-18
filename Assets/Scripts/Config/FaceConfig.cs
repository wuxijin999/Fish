//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class FaceConfig
{

    public readonly string name;
	public readonly int frameCnt;
	public readonly int speed;

    public FaceConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            name = tables[0];

			int.TryParse(tables[1],out frameCnt); 

			int.TryParse(tables[2],out speed); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, FaceConfig> configs = new Dictionary<int, FaceConfig>();
    public static FaceConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        FaceConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new FaceConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Face.txt";
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

			DebugEx.LogFormat("加载结束FaceConfig：{0}",   DateTime.Now);
        });
    }

}




