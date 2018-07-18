//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class HorseUpConfig
{

    public readonly int ID;
	public readonly int HorseID;
	public readonly int LV;
	public readonly int NeedExp;
	public readonly string AttrType;
	public readonly string AttrValue;
	public readonly int[] SkillID;
	public readonly int NeedExpTotal;

    public HorseUpConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out HorseID); 

			int.TryParse(tables[2],out LV); 

			int.TryParse(tables[3],out NeedExp); 

			AttrType = tables[4];

			AttrValue = tables[5];

			string[] SkillIDStringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			SkillID = new int[SkillIDStringArray.Length];
			for (int i=0;i<SkillIDStringArray.Length;i++)
			{
				 int.TryParse(SkillIDStringArray[i],out SkillID[i]);
			}

			int.TryParse(tables[7],out NeedExpTotal); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, HorseUpConfig> configs = new Dictionary<int, HorseUpConfig>();
    public static HorseUpConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        HorseUpConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new HorseUpConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "HorseUp.txt";
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

			DebugEx.LogFormat("加载结束HorseUpConfig：{0}",   DateTime.Now);
        });
    }

}




