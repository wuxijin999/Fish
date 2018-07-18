//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class FunctionForecastConfig
{

    public readonly int FuncId;
	public readonly string FuncName;
	public readonly int OpenLevel;
	public readonly string Describe;
	public readonly string FuncIconKey;
	public readonly string DetailDescribe;
	public readonly string OpenDescribe;

    public FunctionForecastConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out FuncId); 

			FuncName = tables[1];

			int.TryParse(tables[2],out OpenLevel); 

			Describe = tables[3];

			FuncIconKey = tables[4];

			DetailDescribe = tables[5];

			OpenDescribe = tables[6];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, FunctionForecastConfig> configs = new Dictionary<int, FunctionForecastConfig>();
    public static FunctionForecastConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        FunctionForecastConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new FunctionForecastConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "FunctionForecast.txt";
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

			DebugEx.LogFormat("加载结束FunctionForecastConfig：{0}",   DateTime.Now);
        });
    }

}




