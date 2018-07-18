//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TreasurePrivilegeConfig
{

    public readonly int PrivilegeID;
	public readonly string Description;
	public readonly string EffectValue;
	public readonly int singleValue;
	public readonly int maxValue;
	public readonly string attr;
	public readonly string itemAward;
	public readonly string Icon;
	public readonly string Name;
	public readonly string targetDescription;

    public TreasurePrivilegeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out PrivilegeID); 

			Description = tables[1];

			EffectValue = tables[2];

			int.TryParse(tables[3],out singleValue); 

			int.TryParse(tables[4],out maxValue); 

			attr = tables[5];

			itemAward = tables[6];

			Icon = tables[7];

			Name = tables[8];

			targetDescription = tables[9];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TreasurePrivilegeConfig> configs = new Dictionary<int, TreasurePrivilegeConfig>();
    public static TreasurePrivilegeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TreasurePrivilegeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TreasurePrivilegeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TreasurePrivilege.txt";
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

			DebugEx.LogFormat("加载结束TreasurePrivilegeConfig：{0}",   DateTime.Now);
        });
    }

}




