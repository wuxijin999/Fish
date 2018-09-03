//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, September 03, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ItemConfig
{

    public readonly int ID;
	public readonly int LV;
	public readonly string ItemName;
	public readonly int Type;
	public readonly int EquipPlace;
	public readonly int CanRepair;
	public readonly int PackCount;
	public readonly int UseLV;
	public readonly int CanSell;
	public readonly int CanTrade;
	public readonly int CanDrop;
	public readonly int CanBind;
	public readonly int CDType;
	public readonly int CDTime;
	public readonly int GoldPrice;
	public readonly int GoldPaperPrice;
	public readonly int SilverPrice;
	public readonly int UseTag;
	public readonly int Effect1;
	public readonly int EffectValueA1;
	public readonly int EffectValueB1;
	public readonly int EffectValueC1;
	public readonly int Effect2;
	public readonly int EffectValueA2;
	public readonly int EffectValueB2;
	public readonly int EffectValueC2;
	public readonly int Effect3;
	public readonly int EffectValueA3;
	public readonly int EffectValueB3;
	public readonly int EffectValueC3;
	public readonly int Effect4;
	public readonly int EffectValueA4;
	public readonly int EffectValueB4;
	public readonly int EffectValueC4;
	public readonly int Effect5;
	public readonly int EffectValueA5;
	public readonly int EffectValueB5;
	public readonly int EffectValueC5;
	public readonly int AddSkill1;
	public readonly int JobLimit;
	public readonly int ItemColor;
	public readonly int StarLevel;
	public readonly int MaxHoleCount;
	public readonly int CanBreak;
	public readonly int MaxEndure;
	public readonly int EndureReduceType;
	public readonly int BindType;
	public readonly int MaxSkillCnt;
	public readonly int ExpireTime;
	public readonly int MaxFitLV;
	public readonly int SuiteiD;
	public readonly string DropinstantEffName;
	public readonly string IconKey;
	public readonly int ChangeOrd;
	public readonly string Description;
	public readonly string QualityName;
	public readonly int QualityEchoType;
	public readonly int LimitSTR;
	public readonly int LimitPHY;
	public readonly int LimitPNE;
	public readonly string Template;
	public readonly int DropItemPattern;
	public readonly int SellTip;
	public readonly int BatchUse;
	public readonly int Jump;
	public readonly int[] GetWay;
	public readonly string ItemTypeName;

    public ItemConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out LV); 

			ItemName = tables[2];

			int.TryParse(tables[3],out Type); 

			int.TryParse(tables[4],out EquipPlace); 

			int.TryParse(tables[5],out CanRepair); 

			int.TryParse(tables[6],out PackCount); 

			int.TryParse(tables[7],out UseLV); 

			int.TryParse(tables[8],out CanSell); 

			int.TryParse(tables[9],out CanTrade); 

			int.TryParse(tables[10],out CanDrop); 

			int.TryParse(tables[11],out CanBind); 

			int.TryParse(tables[12],out CDType); 

			int.TryParse(tables[13],out CDTime); 

			int.TryParse(tables[14],out GoldPrice); 

			int.TryParse(tables[15],out GoldPaperPrice); 

			int.TryParse(tables[16],out SilverPrice); 

			int.TryParse(tables[17],out UseTag); 

			int.TryParse(tables[18],out Effect1); 

			int.TryParse(tables[19],out EffectValueA1); 

			int.TryParse(tables[20],out EffectValueB1); 

			int.TryParse(tables[21],out EffectValueC1); 

			int.TryParse(tables[22],out Effect2); 

			int.TryParse(tables[23],out EffectValueA2); 

			int.TryParse(tables[24],out EffectValueB2); 

			int.TryParse(tables[25],out EffectValueC2); 

			int.TryParse(tables[26],out Effect3); 

			int.TryParse(tables[27],out EffectValueA3); 

			int.TryParse(tables[28],out EffectValueB3); 

			int.TryParse(tables[29],out EffectValueC3); 

			int.TryParse(tables[30],out Effect4); 

			int.TryParse(tables[31],out EffectValueA4); 

			int.TryParse(tables[32],out EffectValueB4); 

			int.TryParse(tables[33],out EffectValueC4); 

			int.TryParse(tables[34],out Effect5); 

			int.TryParse(tables[35],out EffectValueA5); 

			int.TryParse(tables[36],out EffectValueB5); 

			int.TryParse(tables[37],out EffectValueC5); 

			int.TryParse(tables[38],out AddSkill1); 

			int.TryParse(tables[39],out JobLimit); 

			int.TryParse(tables[40],out ItemColor); 

			int.TryParse(tables[41],out StarLevel); 

			int.TryParse(tables[42],out MaxHoleCount); 

			int.TryParse(tables[43],out CanBreak); 

			int.TryParse(tables[44],out MaxEndure); 

			int.TryParse(tables[45],out EndureReduceType); 

			int.TryParse(tables[46],out BindType); 

			int.TryParse(tables[47],out MaxSkillCnt); 

			int.TryParse(tables[48],out ExpireTime); 

			int.TryParse(tables[49],out MaxFitLV); 

			int.TryParse(tables[50],out SuiteiD); 

			DropinstantEffName = tables[51];

			IconKey = tables[52];

			int.TryParse(tables[53],out ChangeOrd); 

			Description = tables[54];

			QualityName = tables[55];

			int.TryParse(tables[56],out QualityEchoType); 

			int.TryParse(tables[57],out LimitSTR); 

			int.TryParse(tables[58],out LimitPHY); 

			int.TryParse(tables[59],out LimitPNE); 

			Template = tables[60];

			int.TryParse(tables[61],out DropItemPattern); 

			int.TryParse(tables[62],out SellTip); 

			int.TryParse(tables[63],out BatchUse); 

			int.TryParse(tables[64],out Jump); 

			string[] GetWayStringArray = tables[65].Trim().Split(StringUtil.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			GetWay = new int[GetWayStringArray.Length];
			for (int i=0;i<GetWayStringArray.Length;i++)
			{
				 int.TryParse(GetWayStringArray[i],out GetWay[i]);
			}

			ItemTypeName = tables[66];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ItemConfig> configs = new Dictionary<int, ItemConfig>();
    public static ItemConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        ItemConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new ItemConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Item.txt";
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
        });
    }

}




