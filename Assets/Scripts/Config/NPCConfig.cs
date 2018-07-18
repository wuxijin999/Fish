//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class NPCConfig
{

    public readonly int NPCID;
	public readonly int NPCType;
	public readonly string MODE;
	public readonly string charName;
	public readonly int NPCLV;
	public readonly float ModleHeight;
	public readonly float ModelRadius;
	public readonly float ModeProportion;
	public readonly Vector3 UIModeLOffset;
	public readonly float UIModeLProportion;
	public readonly Vector3 UIModelRotation;
	public readonly int CanDeadFly;
	public readonly int Country;
	public readonly int MinAtk;
	public readonly int MaxAtk;
	public readonly int Def;
	public readonly int PoisionAtk;
	public readonly int FireAtk;
	public readonly int IceAtk;
	public readonly int PoisionDef;
	public readonly int IceDef;
	public readonly int AtkInterval;
	public readonly int Hit;
	public readonly int MissRate;
	public readonly int SuperHiteRate;
	public readonly int OrgSpeed;
	public readonly int MoveType;
	public readonly int AtkDist;
	public readonly int Skill1;
	public readonly int Skill2;
	public readonly int Skill3;
	public readonly int Skill4;
	public readonly int Skill5;
	public readonly int Skill6;
	public readonly int Skill7;
	public readonly int Skill8;
	public readonly int AtkType;
	public readonly int Sight;
	public readonly int MoveArea;
	public readonly int DHP;
	public readonly int MaxHPEx;
	public readonly int IsBoss;
	public readonly int SP;
	public readonly int AIType;
	public readonly int CanAttack;
	public readonly float weight;
	public readonly string HeadPortrait;
	public readonly int Show;
	public readonly int AtkFeedback;
	public readonly int hurtFeedback;
	public readonly int AutomaticFace;
	public readonly int Dig;
	public readonly int[] Sounds;
	public readonly int LifeBarCount;
	public readonly int NPCEffect;
	public readonly int NPCSpeakID;

    public NPCConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out NPCID); 

			int.TryParse(tables[1],out NPCType); 

			MODE = tables[2];

			charName = tables[3];

			int.TryParse(tables[4],out NPCLV); 

			float.TryParse(tables[5],out ModleHeight); 

			float.TryParse(tables[6],out ModelRadius); 

			float.TryParse(tables[7],out ModeProportion); 

			UIModeLOffset=tables[8].Vector3Parse();

			float.TryParse(tables[9],out UIModeLProportion); 

			UIModelRotation=tables[10].Vector3Parse();

			int.TryParse(tables[11],out CanDeadFly); 

			int.TryParse(tables[12],out Country); 

			int.TryParse(tables[13],out MinAtk); 

			int.TryParse(tables[14],out MaxAtk); 

			int.TryParse(tables[15],out Def); 

			int.TryParse(tables[16],out PoisionAtk); 

			int.TryParse(tables[17],out FireAtk); 

			int.TryParse(tables[18],out IceAtk); 

			int.TryParse(tables[19],out PoisionDef); 

			int.TryParse(tables[20],out IceDef); 

			int.TryParse(tables[21],out AtkInterval); 

			int.TryParse(tables[22],out Hit); 

			int.TryParse(tables[23],out MissRate); 

			int.TryParse(tables[24],out SuperHiteRate); 

			int.TryParse(tables[25],out OrgSpeed); 

			int.TryParse(tables[26],out MoveType); 

			int.TryParse(tables[27],out AtkDist); 

			int.TryParse(tables[28],out Skill1); 

			int.TryParse(tables[29],out Skill2); 

			int.TryParse(tables[30],out Skill3); 

			int.TryParse(tables[31],out Skill4); 

			int.TryParse(tables[32],out Skill5); 

			int.TryParse(tables[33],out Skill6); 

			int.TryParse(tables[34],out Skill7); 

			int.TryParse(tables[35],out Skill8); 

			int.TryParse(tables[36],out AtkType); 

			int.TryParse(tables[37],out Sight); 

			int.TryParse(tables[38],out MoveArea); 

			int.TryParse(tables[39],out DHP); 

			int.TryParse(tables[40],out MaxHPEx); 

			int.TryParse(tables[41],out IsBoss); 

			int.TryParse(tables[42],out SP); 

			int.TryParse(tables[43],out AIType); 

			int.TryParse(tables[44],out CanAttack); 

			float.TryParse(tables[45],out weight); 

			HeadPortrait = tables[46];

			int.TryParse(tables[47],out Show); 

			int.TryParse(tables[48],out AtkFeedback); 

			int.TryParse(tables[49],out hurtFeedback); 

			int.TryParse(tables[50],out AutomaticFace); 

			int.TryParse(tables[51],out Dig); 

			string[] SoundsStringArray = tables[52].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Sounds = new int[SoundsStringArray.Length];
			for (int i=0;i<SoundsStringArray.Length;i++)
			{
				 int.TryParse(SoundsStringArray[i],out Sounds[i]);
			}

			int.TryParse(tables[53],out LifeBarCount); 

			int.TryParse(tables[54],out NPCEffect); 

			int.TryParse(tables[55],out NPCSpeakID); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, NPCConfig> configs = new Dictionary<int, NPCConfig>();
    public static NPCConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        NPCConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new NPCConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "NPC.txt";
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

			DebugEx.LogFormat("加载结束NPCConfig：{0}",   DateTime.Now);
        });
    }

}




