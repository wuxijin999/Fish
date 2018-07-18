//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class MonsterRefreshPointConfig
{

    public readonly int NPCID;
	public readonly int MapId;
	public readonly Vector3 Position;
	public readonly int scope;
	public readonly int turned;

    public MonsterRefreshPointConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out NPCID); 

			int.TryParse(tables[1],out MapId); 

			Position=tables[2].Vector3Parse();

			int.TryParse(tables[3],out scope); 

			int.TryParse(tables[4],out turned); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, MonsterRefreshPointConfig> configs = new Dictionary<int, MonsterRefreshPointConfig>();
    public static MonsterRefreshPointConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        MonsterRefreshPointConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new MonsterRefreshPointConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "MonsterRefreshPoint.txt";
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

			DebugEx.LogFormat("加载结束MonsterRefreshPointConfig：{0}",   DateTime.Now);
        });
    }

}




