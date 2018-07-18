//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RedPackConfig
{

    public readonly int id;
	public readonly int RedEnvelopeType;
	public readonly int RedEnvelopeAmount;
	public readonly int MoneyType;
	public readonly int PacketCnt;
	public readonly int LeaderOwn;

    public RedPackConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out RedEnvelopeType); 

			int.TryParse(tables[2],out RedEnvelopeAmount); 

			int.TryParse(tables[3],out MoneyType); 

			int.TryParse(tables[4],out PacketCnt); 

			int.TryParse(tables[5],out LeaderOwn); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RedPackConfig> configs = new Dictionary<int, RedPackConfig>();
    public static RedPackConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RedPackConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RedPackConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "RedPack.txt";
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

			DebugEx.LogFormat("加载结束RedPackConfig：{0}",   DateTime.Now);
        });
    }

}




