//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TaskListConfig
{

    public readonly int TaskID;
	public readonly int ChapterID;
	public readonly string ChapterName;
	public readonly string TaskName;
	public readonly string TaskDescribe;
	public readonly string TaskRewards;
	public readonly string TaskTarget;
	public readonly int[] CollectNPC;
	public readonly int FabaoID;

    public TaskListConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out TaskID); 

			int.TryParse(tables[1],out ChapterID); 

			ChapterName = tables[2];

			TaskName = tables[3];

			TaskDescribe = tables[4];

			TaskRewards = tables[5];

			TaskTarget = tables[6];

			string[] CollectNPCStringArray = tables[7].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			CollectNPC = new int[CollectNPCStringArray.Length];
			for (int i=0;i<CollectNPCStringArray.Length;i++)
			{
				 int.TryParse(CollectNPCStringArray[i],out CollectNPC[i]);
			}

			int.TryParse(tables[8],out FabaoID); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TaskListConfig> configs = new Dictionary<int, TaskListConfig>();
    public static TaskListConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TaskListConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TaskListConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TaskList.txt";
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

			DebugEx.LogFormat("加载结束TaskListConfig：{0}",   DateTime.Now);
        });
    }

}




