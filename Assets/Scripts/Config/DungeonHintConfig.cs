//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DungeonHintConfig
{

    public readonly int ID;
	public readonly int dataMapId;
	public readonly int LineId;
	public readonly int targetNum;
	public readonly string[] targetDescription1;
	public readonly int targetType1;
	public readonly int[] NPC1ID;
	public readonly int[] targetValue1;
	public readonly string[] targetDescription2;
	public readonly int targetType2;
	public readonly int[] NPC2ID;
	public readonly int[] targetValue2;
	public readonly string[] targetDescription3;
	public readonly int targetType3;
	public readonly int[] NPC3ID;
	public readonly int[] targetValue3;
	public readonly string[] Info;
	public readonly string mark;

    public DungeonHintConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out dataMapId); 

			int.TryParse(tables[2],out LineId); 

			int.TryParse(tables[3],out targetNum); 

			targetDescription1 = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			int.TryParse(tables[5],out targetType1); 

			string[] NPC1IDStringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			NPC1ID = new int[NPC1IDStringArray.Length];
			for (int i=0;i<NPC1IDStringArray.Length;i++)
			{
				 int.TryParse(NPC1IDStringArray[i],out NPC1ID[i]);
			}

			string[] targetValue1StringArray = tables[7].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			targetValue1 = new int[targetValue1StringArray.Length];
			for (int i=0;i<targetValue1StringArray.Length;i++)
			{
				 int.TryParse(targetValue1StringArray[i],out targetValue1[i]);
			}

			targetDescription2 = tables[8].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			int.TryParse(tables[9],out targetType2); 

			string[] NPC2IDStringArray = tables[10].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			NPC2ID = new int[NPC2IDStringArray.Length];
			for (int i=0;i<NPC2IDStringArray.Length;i++)
			{
				 int.TryParse(NPC2IDStringArray[i],out NPC2ID[i]);
			}

			string[] targetValue2StringArray = tables[11].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			targetValue2 = new int[targetValue2StringArray.Length];
			for (int i=0;i<targetValue2StringArray.Length;i++)
			{
				 int.TryParse(targetValue2StringArray[i],out targetValue2[i]);
			}

			targetDescription3 = tables[12].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			int.TryParse(tables[13],out targetType3); 

			string[] NPC3IDStringArray = tables[14].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			NPC3ID = new int[NPC3IDStringArray.Length];
			for (int i=0;i<NPC3IDStringArray.Length;i++)
			{
				 int.TryParse(NPC3IDStringArray[i],out NPC3ID[i]);
			}

			string[] targetValue3StringArray = tables[15].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			targetValue3 = new int[targetValue3StringArray.Length];
			for (int i=0;i<targetValue3StringArray.Length;i++)
			{
				 int.TryParse(targetValue3StringArray[i],out targetValue3[i]);
			}

			Info = tables[16].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			mark = tables[17];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DungeonHintConfig> configs = new Dictionary<int, DungeonHintConfig>();
    public static DungeonHintConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DungeonHintConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DungeonHintConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "DungeonHint.txt";
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

			DebugEx.LogFormat("加载结束DungeonHintConfig：{0}",   DateTime.Now);
        });
    }

}




