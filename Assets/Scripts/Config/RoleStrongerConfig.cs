//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RoleStrongerConfig
{

    public readonly int id;
	public readonly int funcType;
	public readonly string Name;
	public readonly int funcId;
	public readonly int LV;
	public readonly int[] conditions;
	public readonly int targetValue;
	public readonly int recommendFightPower;
	public readonly int strongerFightPower;
	public readonly string title;
	public readonly string desc;
	public readonly string Icon;
	public readonly int OpenUI;

    public RoleStrongerConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out funcType); 

			Name = tables[2];

			int.TryParse(tables[3],out funcId); 

			int.TryParse(tables[4],out LV); 

			string[] conditionsStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			conditions = new int[conditionsStringArray.Length];
			for (int i=0;i<conditionsStringArray.Length;i++)
			{
				 int.TryParse(conditionsStringArray[i],out conditions[i]);
			}

			int.TryParse(tables[6],out targetValue); 

			int.TryParse(tables[7],out recommendFightPower); 

			int.TryParse(tables[8],out strongerFightPower); 

			title = tables[9];

			desc = tables[10];

			Icon = tables[11];

			int.TryParse(tables[12],out OpenUI); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RoleStrongerConfig> configs = new Dictionary<int, RoleStrongerConfig>();
    public static RoleStrongerConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RoleStrongerConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RoleStrongerConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "RoleStronger.txt";
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

			DebugEx.LogFormat("加载结束RoleStrongerConfig：{0}",   DateTime.Now);
        });
    }

}




