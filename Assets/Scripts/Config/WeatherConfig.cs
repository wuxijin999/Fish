//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class WeatherConfig
{

    public readonly int MapId;
	public readonly int[] EffectIds;
	public readonly int[] EffectDurings;

    public WeatherConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out MapId); 

			string[] EffectIdsStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			EffectIds = new int[EffectIdsStringArray.Length];
			for (int i=0;i<EffectIdsStringArray.Length;i++)
			{
				 int.TryParse(EffectIdsStringArray[i],out EffectIds[i]);
			}

			string[] EffectDuringsStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			EffectDurings = new int[EffectDuringsStringArray.Length];
			for (int i=0;i<EffectDuringsStringArray.Length;i++)
			{
				 int.TryParse(EffectDuringsStringArray[i],out EffectDurings[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, WeatherConfig> configs = new Dictionary<int, WeatherConfig>();
    public static WeatherConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        WeatherConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new WeatherConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Weather.txt";
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

			DebugEx.LogFormat("加载结束WeatherConfig：{0}",   DateTime.Now);
        });
    }

}




