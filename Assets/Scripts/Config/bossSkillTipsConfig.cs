//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class bossSkillTipsConfig
{

    public readonly int SkillID;
	public readonly string Content;
	public readonly int time;
	public readonly int type;
	public readonly int mode;
	public readonly string art;
	public readonly string[] bossIcon;

    public bossSkillTipsConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out SkillID); 

			Content = tables[1];

			int.TryParse(tables[2],out time); 

			int.TryParse(tables[3],out type); 

			int.TryParse(tables[4],out mode); 

			art = tables[5];

			bossIcon = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, bossSkillTipsConfig> configs = new Dictionary<int, bossSkillTipsConfig>();
    public static bossSkillTipsConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        bossSkillTipsConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new bossSkillTipsConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "bossSkillTips.txt";
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

			DebugEx.LogFormat("加载结束bossSkillTipsConfig：{0}",   DateTime.Now);
        });
    }

}




