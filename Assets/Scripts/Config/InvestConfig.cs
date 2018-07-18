//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class InvestConfig
{

    public readonly int id;
	public readonly int type;
	public readonly int needDay;
	public readonly int needLV;
	public readonly string award;

    public InvestConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out type); 

			int.TryParse(tables[2],out needDay); 

			int.TryParse(tables[3],out needLV); 

			award = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, InvestConfig> configs = new Dictionary<int, InvestConfig>();
    public static InvestConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        InvestConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new InvestConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Invest.txt";
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

			DebugEx.LogFormat("加载结束InvestConfig：{0}",   DateTime.Now);
        });
    }

}




