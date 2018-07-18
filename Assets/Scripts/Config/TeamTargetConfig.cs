//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TeamTargetConfig
{

    public readonly int DataMapID;
	public readonly string FBName;
	public readonly int[] TeamTargetUseLineID;
	public readonly int AutoMatchType;
	public readonly int TeamPrepareType;

    public TeamTargetConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out DataMapID); 

			FBName = tables[1];

			string[] TeamTargetUseLineIDStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			TeamTargetUseLineID = new int[TeamTargetUseLineIDStringArray.Length];
			for (int i=0;i<TeamTargetUseLineIDStringArray.Length;i++)
			{
				 int.TryParse(TeamTargetUseLineIDStringArray[i],out TeamTargetUseLineID[i]);
			}

			int.TryParse(tables[3],out AutoMatchType); 

			int.TryParse(tables[4],out TeamPrepareType); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TeamTargetConfig> configs = new Dictionary<int, TeamTargetConfig>();
    public static TeamTargetConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TeamTargetConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TeamTargetConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TeamTarget.txt";
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

			DebugEx.LogFormat("加载结束TeamTargetConfig：{0}",   DateTime.Now);
        });
    }

}




