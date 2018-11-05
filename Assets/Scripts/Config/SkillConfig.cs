//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, November 05, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class SkillConfig
{

    public readonly int id;
	public readonly int name;
	public readonly int attackType;
	public readonly int castType;
	public readonly bool needTarget;
	public readonly int distance;
	public readonly int description;

    public SkillConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out name); 

			int.TryParse(tables[2],out attackType); 

			int.TryParse(tables[3],out castType); 

			var needTargetTemp = 0;
			int.TryParse(tables[4],out needTargetTemp); 
			needTarget=needTargetTemp!=0;

			int.TryParse(tables[5],out distance); 

			int.TryParse(tables[6],out description); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, SkillConfig> configs = new Dictionary<int, SkillConfig>();
    public static SkillConfig Get(int id)
    {   
		if (!inited)
        {
            Debug.Log("SkillConfigConfig 还未完成初始化。");
            return null;
        }
		
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        SkillConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new SkillConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }

	public static bool Has(int id)
    {
        return configs.ContainsKey(id);
    }

	static bool inited = false;
    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
	    inited = false;
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Skill.txt";
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

			inited=true;
        });
    }

}




