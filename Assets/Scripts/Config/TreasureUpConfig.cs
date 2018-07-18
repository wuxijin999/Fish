//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TreasureUpConfig
{

    public readonly int ID;
	public readonly int MWID;
	public readonly int LV;
	public readonly int NeedExp;
	public readonly string AddAttr;
	public readonly int[] UnLockSkill;
	public readonly int UnLockFuncID;
	public readonly int Privilege;
	public readonly int LVLimit;

    public TreasureUpConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out MWID); 

			int.TryParse(tables[2],out LV); 

			int.TryParse(tables[3],out NeedExp); 

			AddAttr = tables[4];

			string[] UnLockSkillStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			UnLockSkill = new int[UnLockSkillStringArray.Length];
			for (int i=0;i<UnLockSkillStringArray.Length;i++)
			{
				 int.TryParse(UnLockSkillStringArray[i],out UnLockSkill[i]);
			}

			int.TryParse(tables[6],out UnLockFuncID); 

			int.TryParse(tables[7],out Privilege); 

			int.TryParse(tables[8],out LVLimit); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TreasureUpConfig> configs = new Dictionary<int, TreasureUpConfig>();
    public static TreasureUpConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TreasureUpConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TreasureUpConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "TreasureUp.txt";
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

			DebugEx.LogFormat("加载结束TreasureUpConfig：{0}",   DateTime.Now);
        });
    }

}




