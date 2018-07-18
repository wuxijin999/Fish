//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class SysInfoConfig
{

    public readonly string key;
	public readonly string sound;
	public readonly string effect;
	public readonly int[] type;
	public readonly string richText;
	public readonly int order;

    public SysInfoConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            key = tables[0];

			sound = tables[1];

			effect = tables[2];

			string[] typeStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			type = new int[typeStringArray.Length];
			for (int i=0;i<typeStringArray.Length;i++)
			{
				 int.TryParse(typeStringArray[i],out type[i]);
			}

			richText = tables[4];

			int.TryParse(tables[5],out order); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, SysInfoConfig> configs = new Dictionary<int, SysInfoConfig>();
    public static SysInfoConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        SysInfoConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new SysInfoConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "SysInfo.txt";
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

			DebugEx.LogFormat("加载结束SysInfoConfig：{0}",   DateTime.Now);
        });
    }

}




