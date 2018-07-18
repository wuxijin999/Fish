//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class GetItemWaysConfig
{

    public readonly int ID;
	public readonly string name;
	public readonly string Icon;
	public readonly string Text;
	public readonly string SelectActive;
	public readonly int OpenpanelId;
	public readonly int FuncOpenId;

    public GetItemWaysConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			name = tables[1];

			Icon = tables[2];

			Text = tables[3];

			SelectActive = tables[4];

			int.TryParse(tables[5],out OpenpanelId); 

			int.TryParse(tables[6],out FuncOpenId); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, GetItemWaysConfig> configs = new Dictionary<int, GetItemWaysConfig>();
    public static GetItemWaysConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        GetItemWaysConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new GetItemWaysConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "GetItemWays.txt";
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

			DebugEx.LogFormat("加载结束GetItemWaysConfig：{0}",   DateTime.Now);
        });
    }

}




