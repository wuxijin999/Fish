//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RuneConfig
{

    public readonly int ID;
	public readonly int[] AttrType;
	public readonly int TowerID;

    public RuneConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			string[] AttrTypeStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AttrType = new int[AttrTypeStringArray.Length];
			for (int i=0;i<AttrTypeStringArray.Length;i++)
			{
				 int.TryParse(AttrTypeStringArray[i],out AttrType[i]);
			}

			int.TryParse(tables[2],out TowerID); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RuneConfig> configs = new Dictionary<int, RuneConfig>();
    public static RuneConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RuneConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RuneConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Rune.txt";
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

			DebugEx.LogFormat("加载结束RuneConfig：{0}",   DateTime.Now);
        });
    }

}




