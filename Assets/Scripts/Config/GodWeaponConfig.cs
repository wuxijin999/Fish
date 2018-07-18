//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class GodWeaponConfig
{

    public readonly int ID;
	public readonly int Type;
	public readonly string Name;
	public readonly int Lv;
	public readonly int NeedExp;
	public readonly int[] AttrType;
	public readonly int[] AttrNum;
	public readonly int SkillID;

    public GodWeaponConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out Type); 

			Name = tables[2];

			int.TryParse(tables[3],out Lv); 

			int.TryParse(tables[4],out NeedExp); 

			string[] AttrTypeStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AttrType = new int[AttrTypeStringArray.Length];
			for (int i=0;i<AttrTypeStringArray.Length;i++)
			{
				 int.TryParse(AttrTypeStringArray[i],out AttrType[i]);
			}

			string[] AttrNumStringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AttrNum = new int[AttrNumStringArray.Length];
			for (int i=0;i<AttrNumStringArray.Length;i++)
			{
				 int.TryParse(AttrNumStringArray[i],out AttrNum[i]);
			}

			int.TryParse(tables[7],out SkillID); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, GodWeaponConfig> configs = new Dictionary<int, GodWeaponConfig>();
    public static GodWeaponConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        GodWeaponConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new GodWeaponConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "GodWeapon.txt";
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

			DebugEx.LogFormat("加载结束GodWeaponConfig：{0}",   DateTime.Now);
        });
    }

}




