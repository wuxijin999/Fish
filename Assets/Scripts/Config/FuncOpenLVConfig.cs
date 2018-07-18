//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class FuncOpenLVConfig
{

    public readonly int FuncId;
	public readonly int LimitLV;
	public readonly int LimitMagicWeapon;
	public readonly int LimiRealmLV;
	public readonly int LimitMissionID;
	public readonly string Remark;
	public readonly string State;
	public readonly string Tip;
	public readonly string Icon;
	public readonly int open;
	public readonly int ContinueTask;

    public FuncOpenLVConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out FuncId); 

			int.TryParse(tables[1],out LimitLV); 

			int.TryParse(tables[2],out LimitMagicWeapon); 

			int.TryParse(tables[3],out LimiRealmLV); 

			int.TryParse(tables[4],out LimitMissionID); 

			Remark = tables[5];

			State = tables[6];

			Tip = tables[7];

			Icon = tables[8];

			int.TryParse(tables[9],out open); 

			int.TryParse(tables[10],out ContinueTask); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, FuncOpenLVConfig> configs = new Dictionary<int, FuncOpenLVConfig>();
    public static FuncOpenLVConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        FuncOpenLVConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new FuncOpenLVConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "FuncOpenLV.txt";
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

			DebugEx.LogFormat("加载结束FuncOpenLVConfig：{0}",   DateTime.Now);
        });
    }

}




