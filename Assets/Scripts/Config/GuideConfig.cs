//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class GuideConfig
{

    public readonly int ID;
	public readonly int Type;
	public readonly int TriggerType;
	public readonly int Condition;
	public readonly int PreGuideId;
	public readonly int[] Steps;
	public readonly int CanSkip;
	public readonly int RemoveWhenOtherGuide;

    public GuideConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out Type); 

			int.TryParse(tables[2],out TriggerType); 

			int.TryParse(tables[3],out Condition); 

			int.TryParse(tables[4],out PreGuideId); 

			string[] StepsStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Steps = new int[StepsStringArray.Length];
			for (int i=0;i<StepsStringArray.Length;i++)
			{
				 int.TryParse(StepsStringArray[i],out Steps[i]);
			}

			int.TryParse(tables[6],out CanSkip); 

			int.TryParse(tables[7],out RemoveWhenOtherGuide); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, GuideConfig> configs = new Dictionary<int, GuideConfig>();
    public static GuideConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        GuideConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new GuideConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Guide.txt";
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

			DebugEx.LogFormat("加载结束GuideConfig：{0}",   DateTime.Now);
        });
    }

}




