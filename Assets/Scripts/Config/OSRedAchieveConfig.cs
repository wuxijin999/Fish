//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class OSRedAchieveConfig
{

    public readonly int id;
	public readonly string typeName;
	public readonly int[] Achieves;
	public readonly int func;
	public readonly string Icon;

    public OSRedAchieveConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			typeName = tables[1];

			string[] AchievesStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Achieves = new int[AchievesStringArray.Length];
			for (int i=0;i<AchievesStringArray.Length;i++)
			{
				 int.TryParse(AchievesStringArray[i],out Achieves[i]);
			}

			int.TryParse(tables[3],out func); 

			Icon = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, OSRedAchieveConfig> configs = new Dictionary<int, OSRedAchieveConfig>();
    public static OSRedAchieveConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        OSRedAchieveConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new OSRedAchieveConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "OSRedAchieve.txt";
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

			DebugEx.LogFormat("加载结束OSRedAchieveConfig：{0}",   DateTime.Now);
        });
    }

}




