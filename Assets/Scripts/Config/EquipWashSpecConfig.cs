//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EquipWashSpecConfig
{

    public readonly int id;
	public readonly int typeNeed;
	public readonly int levelNeed;
	public readonly string attByLevel;
	public readonly string attByLevelValue;
	public readonly int MasterLV;

    public EquipWashSpecConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out typeNeed); 

			int.TryParse(tables[2],out levelNeed); 

			attByLevel = tables[3];

			attByLevelValue = tables[4];

			int.TryParse(tables[5],out MasterLV); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EquipWashSpecConfig> configs = new Dictionary<int, EquipWashSpecConfig>();
    public static EquipWashSpecConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        EquipWashSpecConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new EquipWashSpecConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "EquipWashSpec.txt";
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

			DebugEx.LogFormat("加载结束EquipWashSpecConfig：{0}",   DateTime.Now);
        });
    }

}




