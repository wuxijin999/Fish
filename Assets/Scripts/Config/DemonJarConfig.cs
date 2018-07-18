//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DemonJarConfig
{

    public readonly int NPCID;
	public readonly int LineID;
	public readonly int Time;
	public readonly int[] ParticipateItemID;
	public readonly int[] RareItemID;
	public readonly string PortraitID;
	public readonly int SpecialItemMark;
	public readonly int CanEnterTimes;
	public readonly int AutoAttention;
	public readonly int[] Job1;
	public readonly int[] Job2;
	public readonly int[] Job3;
	public readonly int KillHurtMin;
	public readonly int KillHurtMax;

    public DemonJarConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out NPCID); 

			int.TryParse(tables[1],out LineID); 

			int.TryParse(tables[2],out Time); 

			string[] ParticipateItemIDStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ParticipateItemID = new int[ParticipateItemIDStringArray.Length];
			for (int i=0;i<ParticipateItemIDStringArray.Length;i++)
			{
				 int.TryParse(ParticipateItemIDStringArray[i],out ParticipateItemID[i]);
			}

			string[] RareItemIDStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			RareItemID = new int[RareItemIDStringArray.Length];
			for (int i=0;i<RareItemIDStringArray.Length;i++)
			{
				 int.TryParse(RareItemIDStringArray[i],out RareItemID[i]);
			}

			PortraitID = tables[5];

			int.TryParse(tables[6],out SpecialItemMark); 

			int.TryParse(tables[7],out CanEnterTimes); 

			int.TryParse(tables[8],out AutoAttention); 

			string[] Job1StringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Job1 = new int[Job1StringArray.Length];
			for (int i=0;i<Job1StringArray.Length;i++)
			{
				 int.TryParse(Job1StringArray[i],out Job1[i]);
			}

			string[] Job2StringArray = tables[10].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Job2 = new int[Job2StringArray.Length];
			for (int i=0;i<Job2StringArray.Length;i++)
			{
				 int.TryParse(Job2StringArray[i],out Job2[i]);
			}

			string[] Job3StringArray = tables[11].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Job3 = new int[Job3StringArray.Length];
			for (int i=0;i<Job3StringArray.Length;i++)
			{
				 int.TryParse(Job3StringArray[i],out Job3[i]);
			}

			int.TryParse(tables[12],out KillHurtMin); 

			int.TryParse(tables[13],out KillHurtMax); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DemonJarConfig> configs = new Dictionary<int, DemonJarConfig>();
    public static DemonJarConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DemonJarConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DemonJarConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DemonJar.txt";
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

			DebugEx.LogFormat("加载结束DemonJarConfig：{0}",   DateTime.Now);
        });
    }

}




