//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RuneComposeConfig
{

    public readonly int TagItemID;
	public readonly int[] NeedItem;
	public readonly int NeedMJ;

    public RuneComposeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out TagItemID); 

			string[] NeedItemStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			NeedItem = new int[NeedItemStringArray.Length];
			for (int i=0;i<NeedItemStringArray.Length;i++)
			{
				 int.TryParse(NeedItemStringArray[i],out NeedItem[i]);
			}

			int.TryParse(tables[2],out NeedMJ); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RuneComposeConfig> configs = new Dictionary<int, RuneComposeConfig>();
    public static RuneComposeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RuneComposeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RuneComposeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "RuneCompose.txt";
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

			DebugEx.LogFormat("加载结束RuneComposeConfig：{0}",   DateTime.Now);
        });
    }

}




