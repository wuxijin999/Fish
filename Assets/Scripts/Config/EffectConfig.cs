//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EffectConfig
{

    public readonly int id;
	public readonly string packageName;
	public readonly string fxName;
	public readonly int job;
	public readonly int audio;
	public readonly int stopImmediate;
	public readonly int setParent;
	public readonly string nodeName;

    public EffectConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			packageName = tables[1];

			fxName = tables[2];

			int.TryParse(tables[3],out job); 

			int.TryParse(tables[4],out audio); 

			int.TryParse(tables[5],out stopImmediate); 

			int.TryParse(tables[6],out setParent); 

			nodeName = tables[7];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EffectConfig> configs = new Dictionary<int, EffectConfig>();
    public static EffectConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        EffectConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new EffectConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Effect.txt";
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

			DebugEx.LogFormat("加载结束EffectConfig：{0}",   DateTime.Now);
        });
    }

}




