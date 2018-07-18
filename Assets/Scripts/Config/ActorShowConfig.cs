//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ActorShowConfig
{

    public readonly int ID;
	public readonly int NpcID;
	public readonly int MapID;
	public readonly int line;
	public readonly int[] showNpcs;
	public readonly int length;
	public readonly int showNameTime;
	public readonly int BindMissionID;
	public readonly int type;
	public readonly int[] scale;
	public readonly int[] NpcFace;
	public readonly int[] PosX;
	public readonly int[] PosY;
	public readonly int shadow;
	public readonly int effect;
	public readonly int uieffect;
	public readonly string[] mob;
	public readonly string cam;
	public readonly int[] Height;
	public readonly int DialogueTime;
	public readonly int Dialogue;
	public readonly int soundId;
	public readonly int soundTime;

    public ActorShowConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out NpcID); 

			int.TryParse(tables[2],out MapID); 

			int.TryParse(tables[3],out line); 

			string[] showNpcsStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			showNpcs = new int[showNpcsStringArray.Length];
			for (int i=0;i<showNpcsStringArray.Length;i++)
			{
				 int.TryParse(showNpcsStringArray[i],out showNpcs[i]);
			}

			int.TryParse(tables[5],out length); 

			int.TryParse(tables[6],out showNameTime); 

			int.TryParse(tables[7],out BindMissionID); 

			int.TryParse(tables[8],out type); 

			string[] scaleStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			scale = new int[scaleStringArray.Length];
			for (int i=0;i<scaleStringArray.Length;i++)
			{
				 int.TryParse(scaleStringArray[i],out scale[i]);
			}

			string[] NpcFaceStringArray = tables[10].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			NpcFace = new int[NpcFaceStringArray.Length];
			for (int i=0;i<NpcFaceStringArray.Length;i++)
			{
				 int.TryParse(NpcFaceStringArray[i],out NpcFace[i]);
			}

			string[] PosXStringArray = tables[11].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			PosX = new int[PosXStringArray.Length];
			for (int i=0;i<PosXStringArray.Length;i++)
			{
				 int.TryParse(PosXStringArray[i],out PosX[i]);
			}

			string[] PosYStringArray = tables[12].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			PosY = new int[PosYStringArray.Length];
			for (int i=0;i<PosYStringArray.Length;i++)
			{
				 int.TryParse(PosYStringArray[i],out PosY[i]);
			}

			int.TryParse(tables[13],out shadow); 

			int.TryParse(tables[14],out effect); 

			int.TryParse(tables[15],out uieffect); 

			mob = tables[16].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			cam = tables[17];

			string[] HeightStringArray = tables[18].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Height = new int[HeightStringArray.Length];
			for (int i=0;i<HeightStringArray.Length;i++)
			{
				 int.TryParse(HeightStringArray[i],out Height[i]);
			}

			int.TryParse(tables[19],out DialogueTime); 

			int.TryParse(tables[20],out Dialogue); 

			int.TryParse(tables[21],out soundId); 

			int.TryParse(tables[22],out soundTime); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ActorShowConfig> configs = new Dictionary<int, ActorShowConfig>();
    public static ActorShowConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ActorShowConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ActorShowConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ActorShow.txt";
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

			DebugEx.LogFormat("加载结束ActorShowConfig：{0}",   DateTime.Now);
        });
    }

}




