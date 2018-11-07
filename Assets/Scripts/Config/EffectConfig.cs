//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, November 07, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EffectConfig
{

    public readonly int id;
	public readonly string package;
	public readonly string assetName;
	public readonly bool bindParent;

    public EffectConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			package = tables[1];

			assetName = tables[2];

			var bindParentTemp = 0;
			int.TryParse(tables[3],out bindParentTemp); 
			bindParent=bindParentTemp!=0;
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EffectConfig> configs = new Dictionary<int, EffectConfig>();
    public static EffectConfig Get(int id)
    {   
		if (!inited)
        {
            Debug.Log("EffectConfigConfig 还未完成初始化。");
            return null;
        }
		
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        EffectConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new EffectConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }

	public static bool Has(int id)
    {
        return configs.ContainsKey(id);
    }

	static bool inited = false;
    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
	    inited = false;
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

			inited=true;
        });
    }

}




