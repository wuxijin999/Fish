//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class AreaCameraConfig
{

    public readonly int AreaID;
	public readonly int Distance;
	public readonly int RotX;
	public readonly int RotY;

    public AreaCameraConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out AreaID); 

			int.TryParse(tables[1],out Distance); 

			int.TryParse(tables[2],out RotX); 

			int.TryParse(tables[3],out RotY); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, AreaCameraConfig> configs = new Dictionary<int, AreaCameraConfig>();
    public static AreaCameraConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        AreaCameraConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new AreaCameraConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "AreaCamera.txt";
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

			DebugEx.LogFormat("加载结束AreaCameraConfig：{0}",   DateTime.Now);
        });
    }

}




