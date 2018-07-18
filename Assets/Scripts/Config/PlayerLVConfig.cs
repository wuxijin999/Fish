//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PlayerLVConfig
{

    public readonly int LV;
	public readonly int EXP1;
	public readonly int EXP2;
	public readonly int TalentPoint;
	public readonly int ReExp;
	public readonly int fightPower;

    public PlayerLVConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out LV); 

			int.TryParse(tables[1],out EXP1); 

			int.TryParse(tables[2],out EXP2); 

			int.TryParse(tables[3],out TalentPoint); 

			int.TryParse(tables[4],out ReExp); 

			int.TryParse(tables[5],out fightPower); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PlayerLVConfig> configs = new Dictionary<int, PlayerLVConfig>();
    public static PlayerLVConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        PlayerLVConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new PlayerLVConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "PlayerLV.txt";
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

			DebugEx.LogFormat("加载结束PlayerLVConfig：{0}",   DateTime.Now);
        });
    }

}




