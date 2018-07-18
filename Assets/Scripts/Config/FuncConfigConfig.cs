//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class FuncConfigConfig
{

    public readonly string KEY;
	public readonly string Numerical1;
	public readonly string Numerical2;
	public readonly string Numerical3;
	public readonly string Numerical4;
	public readonly string Numerical5;

    public FuncConfigConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            KEY = tables[0];

			Numerical1 = tables[1];

			Numerical2 = tables[2];

			Numerical3 = tables[3];

			Numerical4 = tables[4];

			Numerical5 = tables[5];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, FuncConfigConfig> configs = new Dictionary<int, FuncConfigConfig>();
    public static FuncConfigConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        FuncConfigConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new FuncConfigConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "FuncConfig.txt";
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

			DebugEx.LogFormat("加载结束FuncConfigConfig：{0}",   DateTime.Now);
        });
    }

}




