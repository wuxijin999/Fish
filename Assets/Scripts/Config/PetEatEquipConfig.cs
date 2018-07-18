//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PetEatEquipConfig
{

    public readonly int ID;
	public readonly int EquipColor;
	public readonly int EquipClass;
	public readonly int Exp;
	public readonly int integrate;

    public PetEatEquipConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out EquipColor); 

			int.TryParse(tables[2],out EquipClass); 

			int.TryParse(tables[3],out Exp); 

			int.TryParse(tables[4],out integrate); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PetEatEquipConfig> configs = new Dictionary<int, PetEatEquipConfig>();
    public static PetEatEquipConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        PetEatEquipConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new PetEatEquipConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "PetEatEquip.txt";
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

			DebugEx.LogFormat("加载结束PetEatEquipConfig：{0}",   DateTime.Now);
        });
    }

}




