//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RealmPracticeConfig
{

    public readonly int ID;
	public readonly int Type;
	public readonly string FirstTypeName;
	public readonly int SecondType;
	public readonly string SecondTypeName;
	public readonly int[] AchieveID;
	public readonly int FunctionID;
	public readonly string Icon;

    public RealmPracticeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out Type); 

			FirstTypeName = tables[2];

			int.TryParse(tables[3],out SecondType); 

			SecondTypeName = tables[4];

			string[] AchieveIDStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AchieveID = new int[AchieveIDStringArray.Length];
			for (int i=0;i<AchieveIDStringArray.Length;i++)
			{
				 int.TryParse(AchieveIDStringArray[i],out AchieveID[i]);
			}

			int.TryParse(tables[6],out FunctionID); 

			Icon = tables[7];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RealmPracticeConfig> configs = new Dictionary<int, RealmPracticeConfig>();
    public static RealmPracticeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RealmPracticeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RealmPracticeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "RealmPractice.txt";
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

			DebugEx.LogFormat("加载结束RealmPracticeConfig：{0}",   DateTime.Now);
        });
    }

}




