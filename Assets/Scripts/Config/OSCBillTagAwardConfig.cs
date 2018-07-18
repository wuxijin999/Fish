﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class OSCBillTagAwardConfig
{

    public readonly int ID;
	public readonly int RangListType;
	public readonly int Condition;
	public readonly string Gift;
	public readonly string Tip;

    public OSCBillTagAwardConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out RangListType); 

			int.TryParse(tables[2],out Condition); 

			Gift = tables[3];

			Tip = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, OSCBillTagAwardConfig> configs = new Dictionary<int, OSCBillTagAwardConfig>();
    public static OSCBillTagAwardConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        OSCBillTagAwardConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new OSCBillTagAwardConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "OSCBillTagAward.txt";
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

			DebugEx.LogFormat("加载结束OSCBillTagAwardConfig：{0}",   DateTime.Now);
        });
    }

}




