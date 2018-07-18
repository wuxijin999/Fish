//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class WingRefineAttrConfig
{

    public readonly int id;
	public readonly int wingsPhase;
	public readonly string attrupper;
	public readonly string EXPquality;
	public readonly int EXPupper;

    public WingRefineAttrConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out wingsPhase); 

			attrupper = tables[2];

			EXPquality = tables[3];

			int.TryParse(tables[4],out EXPupper); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, WingRefineAttrConfig> configs = new Dictionary<int, WingRefineAttrConfig>();
    public static WingRefineAttrConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        WingRefineAttrConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new WingRefineAttrConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "WingRefineAttr.txt";
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

			DebugEx.LogFormat("加载结束WingRefineAttrConfig：{0}",   DateTime.Now);
        });
    }

}




