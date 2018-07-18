//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class SkillConfig
{

    public readonly int SkillID;
	public readonly string SkillName;
	public readonly int SkillTypeID;
	public readonly int SkillLV;
	public readonly int SkillMaxLV;
	public readonly int UseType;
	public readonly int FuncType;
	public readonly int CastTime;
	public readonly int SkillType;
	public readonly int HurtType;
	public readonly int ContinueUse;
	public readonly int AtkType;
	public readonly int AtkRadius;
	public readonly int Tag;
	public readonly int AtkDist;
	public readonly int StiffTime;
	public readonly int CoolDownTime;
	public readonly int MP;
	public readonly int HP;
	public readonly int XP;
	public readonly int UseItemID;
	public readonly int UseItemCount;
	public readonly int Effect1;
	public readonly int EffectValue11;
	public readonly int EffectValue12;
	public readonly int EffectValue13;
	public readonly int Effect2;
	public readonly int EffectValue21;
	public readonly int EffectValue22;
	public readonly int EffectValue23;
	public readonly int Effect3;
	public readonly int EffectValue31;
	public readonly int EffectValue32;
	public readonly int EffectValue33;
	public readonly int Effect4;
	public readonly int EffectValue41;
	public readonly int EffectValue42;
	public readonly int EffectValue43;
	public readonly int Effect5;
	public readonly int EffectValue51;
	public readonly int EffectValue52;
	public readonly int EffectValue53;
	public readonly int Effect6;
	public readonly int EffectValue61;
	public readonly int EffectValue62;
	public readonly int EffectValue63;
	public readonly int LearnSkillReq;
	public readonly int LearnSkillLV;
	public readonly int LearnLVReq;
	public readonly int FightPower;
	public readonly int LVUpCostMoneyType;
	public readonly int LVUpCostMoney;
	public readonly int LVUpCostExp;
	public readonly int ClientActionLimit;
	public readonly int ClientSkillSeriesLimit;
	public readonly int SkillOfSeries;
	public readonly int ExpendMPRate;
	public readonly int ExAttr1;
	public readonly int ExAttr3;
	public readonly int ExAttr4;
	public readonly int ExAttr5;
	public readonly int WarnInfo;
	public readonly int CtrlActionID;
	public readonly int BuffEffectID;
	public readonly int EffectName;
	public readonly string IconName;
	public readonly string SkillNameIcon;
	public readonly string Description;
	public readonly string BuffDescription;
	public readonly string Skillsource;
	public readonly int Skillactmark;
	public readonly int BuffDisplay;

    public SkillConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out SkillID); 

			SkillName = tables[1];

			int.TryParse(tables[2],out SkillTypeID); 

			int.TryParse(tables[3],out SkillLV); 

			int.TryParse(tables[4],out SkillMaxLV); 

			int.TryParse(tables[5],out UseType); 

			int.TryParse(tables[6],out FuncType); 

			int.TryParse(tables[7],out CastTime); 

			int.TryParse(tables[8],out SkillType); 

			int.TryParse(tables[9],out HurtType); 

			int.TryParse(tables[10],out ContinueUse); 

			int.TryParse(tables[11],out AtkType); 

			int.TryParse(tables[12],out AtkRadius); 

			int.TryParse(tables[13],out Tag); 

			int.TryParse(tables[14],out AtkDist); 

			int.TryParse(tables[15],out StiffTime); 

			int.TryParse(tables[16],out CoolDownTime); 

			int.TryParse(tables[17],out MP); 

			int.TryParse(tables[18],out HP); 

			int.TryParse(tables[19],out XP); 

			int.TryParse(tables[20],out UseItemID); 

			int.TryParse(tables[21],out UseItemCount); 

			int.TryParse(tables[22],out Effect1); 

			int.TryParse(tables[23],out EffectValue11); 

			int.TryParse(tables[24],out EffectValue12); 

			int.TryParse(tables[25],out EffectValue13); 

			int.TryParse(tables[26],out Effect2); 

			int.TryParse(tables[27],out EffectValue21); 

			int.TryParse(tables[28],out EffectValue22); 

			int.TryParse(tables[29],out EffectValue23); 

			int.TryParse(tables[30],out Effect3); 

			int.TryParse(tables[31],out EffectValue31); 

			int.TryParse(tables[32],out EffectValue32); 

			int.TryParse(tables[33],out EffectValue33); 

			int.TryParse(tables[34],out Effect4); 

			int.TryParse(tables[35],out EffectValue41); 

			int.TryParse(tables[36],out EffectValue42); 

			int.TryParse(tables[37],out EffectValue43); 

			int.TryParse(tables[38],out Effect5); 

			int.TryParse(tables[39],out EffectValue51); 

			int.TryParse(tables[40],out EffectValue52); 

			int.TryParse(tables[41],out EffectValue53); 

			int.TryParse(tables[42],out Effect6); 

			int.TryParse(tables[43],out EffectValue61); 

			int.TryParse(tables[44],out EffectValue62); 

			int.TryParse(tables[45],out EffectValue63); 

			int.TryParse(tables[46],out LearnSkillReq); 

			int.TryParse(tables[47],out LearnSkillLV); 

			int.TryParse(tables[48],out LearnLVReq); 

			int.TryParse(tables[49],out FightPower); 

			int.TryParse(tables[50],out LVUpCostMoneyType); 

			int.TryParse(tables[51],out LVUpCostMoney); 

			int.TryParse(tables[52],out LVUpCostExp); 

			int.TryParse(tables[53],out ClientActionLimit); 

			int.TryParse(tables[54],out ClientSkillSeriesLimit); 

			int.TryParse(tables[55],out SkillOfSeries); 

			int.TryParse(tables[56],out ExpendMPRate); 

			int.TryParse(tables[57],out ExAttr1); 

			int.TryParse(tables[58],out ExAttr3); 

			int.TryParse(tables[59],out ExAttr4); 

			int.TryParse(tables[60],out ExAttr5); 

			int.TryParse(tables[61],out WarnInfo); 

			int.TryParse(tables[62],out CtrlActionID); 

			int.TryParse(tables[63],out BuffEffectID); 

			int.TryParse(tables[64],out EffectName); 

			IconName = tables[65];

			SkillNameIcon = tables[66];

			Description = tables[67];

			BuffDescription = tables[68];

			Skillsource = tables[69];

			int.TryParse(tables[70],out Skillactmark); 

			int.TryParse(tables[71],out BuffDisplay); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, SkillConfig> configs = new Dictionary<int, SkillConfig>();
    public static SkillConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        SkillConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new SkillConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
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

			DebugEx.LogFormat("加载结束SkillConfig：{0}",   DateTime.Now);
        });
    }

}




