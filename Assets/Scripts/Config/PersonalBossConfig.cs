//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PersonalBossConfig
{

    public readonly int NPCID;
	public readonly int ChanllengeLv;
	public readonly int[] MustItemID;
	public readonly int[] RareItemID;
	public readonly string PortraitID;

    public PersonalBossConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out NPCID); 

			int.TryParse(tables[1],out ChanllengeLv); 

			string[] MustItemIDStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			MustItemID = new int[MustItemIDStringArray.Length];
			for (int i=0;i<MustItemIDStringArray.Length;i++)
			{
				 int.TryParse(MustItemIDStringArray[i],out MustItemID[i]);
			}

			string[] RareItemIDStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RareItemID = new int[RareItemIDStringArray.Length];
			for (int i=0;i<RareItemIDStringArray.Length;i++)
			{
				 int.TryParse(RareItemIDStringArray[i],out RareItemID[i]);
			}

			PortraitID = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PersonalBossConfig> configs = new Dictionary<int, PersonalBossConfig>();
    public static PersonalBossConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        PersonalBossConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new PersonalBossConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "PersonalBoss.txt";
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

			DebugEx.LogFormat("加载结束PersonalBossConfig：{0}",   DateTime.Now);
        });
    }

}




