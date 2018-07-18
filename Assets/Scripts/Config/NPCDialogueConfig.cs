//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class NPCDialogueConfig
{

    public readonly int ID;
	public readonly int NPCID;
	public readonly int DataMapId;
	public readonly int Interval;
	public readonly int NextTime;
	public readonly string[] Dialogues;

    public NPCDialogueConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out NPCID); 

			int.TryParse(tables[2],out DataMapId); 

			int.TryParse(tables[3],out Interval); 

			int.TryParse(tables[4],out NextTime); 

			Dialogues = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, NPCDialogueConfig> configs = new Dictionary<int, NPCDialogueConfig>();
    public static NPCDialogueConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        NPCDialogueConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new NPCDialogueConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "NPCDialogue.txt";
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

			DebugEx.LogFormat("加载结束NPCDialogueConfig：{0}",   DateTime.Now);
        });
    }

}




