//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ChestsConfig
{

    public readonly int BoxID;
	public readonly int ExpendItemID;
	public readonly int ExpendCount;
	public readonly int OpenMoney;
	public readonly int OpenShow;

    public ChestsConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out BoxID); 

			int.TryParse(tables[1],out ExpendItemID); 

			int.TryParse(tables[2],out ExpendCount); 

			int.TryParse(tables[3],out OpenMoney); 

			int.TryParse(tables[4],out OpenShow); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ChestsConfig> configs = new Dictionary<int, ChestsConfig>();
    public static ChestsConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ChestsConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ChestsConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Chests.txt";
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

			DebugEx.LogFormat("加载结束ChestsConfig：{0}",   DateTime.Now);
        });
    }

}




