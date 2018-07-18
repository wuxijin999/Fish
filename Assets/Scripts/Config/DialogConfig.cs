//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DialogConfig
{

    public readonly int id;
	public readonly int npcId;
	public readonly string icon;
	public readonly string name;
	public readonly string content;
	public readonly int nextID;
	public readonly int TalkID;

    public DialogConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out npcId); 

			icon = tables[2];

			name = tables[3];

			content = tables[4];

			int.TryParse(tables[5],out nextID); 

			int.TryParse(tables[6],out TalkID); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DialogConfig> configs = new Dictionary<int, DialogConfig>();
    public static DialogConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DialogConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DialogConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Dialog.txt";
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

			DebugEx.LogFormat("加载结束DialogConfig：{0}",   DateTime.Now);
        });
    }

}




