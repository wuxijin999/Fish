﻿//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, November 05, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TestConfig
{

    public readonly int ID;
	public readonly bool LV;
	public readonly Int2 ItemName;
	public readonly Int3 Type;
	public readonly Vector3 EquipPlace;

    public TestConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out ID); 

			var LVTemp = 0;
			int.TryParse(tables[1],out LVTemp); 
			LV=LVTemp!=0;

			Int2.TryParse(tables[2],out ItemName); 

			Int3.TryParse(tables[3],out Type); 

			EquipPlace=tables[4].Vector3Parse();
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TestConfig> configs = new Dictionary<int, TestConfig>();
    public static TestConfig Get(int id)
    {   
		if (!inited)
        {
            Debug.Log("TestConfigConfig 还未完成初始化。");
            return null;
        }
		
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        TestConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new TestConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }

	public static bool Has(int id)
    {
        return configs.ContainsKey(id);
    }

	static bool inited = false;
    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
	    inited = false;
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Test.txt";
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

			inited=true;
        });
    }

}




