//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class LoadingTipConfig
{

    public readonly int ID;
	public readonly string Content;
	public readonly int MinLevel;
	public readonly int MaxLevel;

    public LoadingTipConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			Content = tables[1];

			int.TryParse(tables[2],out MinLevel); 

			int.TryParse(tables[3],out MaxLevel); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, LoadingTipConfig> configs = new Dictionary<int, LoadingTipConfig>();
    public static LoadingTipConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        LoadingTipConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new LoadingTipConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "LoadingTip.txt";
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

			DebugEx.LogFormat("加载结束LoadingTipConfig：{0}",   DateTime.Now);
        });
    }

}




