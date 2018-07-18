//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TASKINFOConfig
{

    public readonly string code;
	public readonly string show_writing;

    public TASKINFOConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            code = tables[0];

			show_writing = tables[1];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TASKINFOConfig> configs = new Dictionary<int, TASKINFOConfig>();
    public static TASKINFOConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TASKINFOConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TASKINFOConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TASKINFO.txt";
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

			DebugEx.LogFormat("加载结束TASKINFOConfig：{0}",   DateTime.Now);
        });
    }

}




