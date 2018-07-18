//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class XMZZAchievementConfig
{

    public readonly int ID;
	public readonly int[] AchieveID;

    public XMZZAchievementConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			string[] AchieveIDStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AchieveID = new int[AchieveIDStringArray.Length];
			for (int i=0;i<AchieveIDStringArray.Length;i++)
			{
				 int.TryParse(AchieveIDStringArray[i],out AchieveID[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, XMZZAchievementConfig> configs = new Dictionary<int, XMZZAchievementConfig>();
    public static XMZZAchievementConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        XMZZAchievementConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new XMZZAchievementConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "XMZZAchievement.txt";
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

			DebugEx.LogFormat("加载结束XMZZAchievementConfig：{0}",   DateTime.Now);
        });
    }

}




