//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class StoryMissionsConfig
{

    public readonly int TaskID;
	public readonly int[] NpcID;
	public readonly int[] TalkNum;
	public readonly string[] content;
	public readonly int[] Speaker1;
	public readonly int[] Speaker2;
	public readonly string[] NpcIcon;
	public readonly int TaskMusic;

    public StoryMissionsConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out TaskID); 

			string[] NpcIDStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			NpcID = new int[NpcIDStringArray.Length];
			for (int i=0;i<NpcIDStringArray.Length;i++)
			{
				 int.TryParse(NpcIDStringArray[i],out NpcID[i]);
			}

			string[] TalkNumStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			TalkNum = new int[TalkNumStringArray.Length];
			for (int i=0;i<TalkNumStringArray.Length;i++)
			{
				 int.TryParse(TalkNumStringArray[i],out TalkNum[i]);
			}

			content = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			string[] Speaker1StringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Speaker1 = new int[Speaker1StringArray.Length];
			for (int i=0;i<Speaker1StringArray.Length;i++)
			{
				 int.TryParse(Speaker1StringArray[i],out Speaker1[i]);
			}

			string[] Speaker2StringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Speaker2 = new int[Speaker2StringArray.Length];
			for (int i=0;i<Speaker2StringArray.Length;i++)
			{
				 int.TryParse(Speaker2StringArray[i],out Speaker2[i]);
			}

			NpcIcon = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			int.TryParse(tables[7],out TaskMusic); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, StoryMissionsConfig> configs = new Dictionary<int, StoryMissionsConfig>();
    public static StoryMissionsConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        StoryMissionsConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new StoryMissionsConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "StoryMissions.txt";
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

			DebugEx.LogFormat("加载结束StoryMissionsConfig：{0}",   DateTime.Now);
        });
    }

}




