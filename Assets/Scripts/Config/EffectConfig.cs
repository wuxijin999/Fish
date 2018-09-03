//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, September 03, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EffectConfig
{

    public readonly int id;
	public readonly string assetName;
	public readonly bool bindParent;

    public EffectConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			assetName = tables[1];

			var bindParentTemp = 0;
			int.TryParse(tables[2],out bindParentTemp); 
			bindParent=bindParentTemp!=0;
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
        });
    }

}




