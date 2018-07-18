//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class TreasureConfig
{

    public readonly int ID;
	public readonly int Category;
	public readonly int PreTreasure;
	public readonly string Name;
	public readonly string Icon;
	public readonly string NameIcon;
	public readonly string TreasureNameIcon;
	public readonly string Model;
	public readonly string SourceDescription;
	public readonly string Story;
	public readonly string IndexTitle;
	public readonly string StoryName;
	public readonly int RequirementTotal;
	public readonly int[] Achievements;
	public readonly int MapId;
	public readonly int LineId;
	public readonly int ChallengeLevel;
	public readonly int[] Potentials;
	public readonly string NeedItem;
	public readonly int EffectID;
	public readonly string[] Verse;
	public readonly int ShowNetGotEffect;
	public readonly int PreferredStage;

    public TreasureConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out Category); 

			int.TryParse(tables[2],out PreTreasure); 

			Name = tables[3];

			Icon = tables[4];

			NameIcon = tables[5];

			TreasureNameIcon = tables[6];

			Model = tables[7];

			SourceDescription = tables[8];

			Story = tables[9];

			IndexTitle = tables[10];

			StoryName = tables[11];

			int.TryParse(tables[12],out RequirementTotal); 

			string[] AchievementsStringArray = tables[13].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Achievements = new int[AchievementsStringArray.Length];
			for (int i=0;i<AchievementsStringArray.Length;i++)
			{
				 int.TryParse(AchievementsStringArray[i],out Achievements[i]);
			}

			int.TryParse(tables[14],out MapId); 

			int.TryParse(tables[15],out LineId); 

			int.TryParse(tables[16],out ChallengeLevel); 

			string[] PotentialsStringArray = tables[17].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Potentials = new int[PotentialsStringArray.Length];
			for (int i=0;i<PotentialsStringArray.Length;i++)
			{
				 int.TryParse(PotentialsStringArray[i],out Potentials[i]);
			}

			NeedItem = tables[18];

			int.TryParse(tables[19],out EffectID); 

			Verse = tables[20].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			int.TryParse(tables[21],out ShowNetGotEffect); 

			int.TryParse(tables[22],out PreferredStage); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, TreasureConfig> configs = new Dictionary<int, TreasureConfig>();
    public static TreasureConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        TreasureConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new TreasureConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Treasure.txt";
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

			DebugEx.LogFormat("加载结束TreasureConfig：{0}",   DateTime.Now);
        });
    }

}




