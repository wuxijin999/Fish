//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class EquipGSParamConfig
{

    public readonly int ID;
	public readonly int EquipClass;
	public readonly int EquipColor;
	public readonly int EquipStar;
	public readonly int AtkPerC;
	public readonly int DamagePerC;
	public readonly int SuperHitRateC;
	public readonly int SuperHitPerC;
	public readonly int DamReduceC;
	public readonly int MaxHPPerC;
	public readonly int DefPerC;
	public readonly int LuckyHitRateC;
	public readonly int PetDamPerC;
	public readonly int PerLVAtkC;
	public readonly int MissRateC;
	public readonly int HitRateC;
	public readonly int DamBackPerC;
	public readonly int PerLVMaxHPC;
	public readonly int DropEquipPerC;
	public readonly int DropMoneyPerC;
	public readonly int IgnoreDefRateReduceC;
	public readonly int DamChanceDefC;
	public readonly int SuperHitReduceC;
	public readonly int SkillAtkRateC;
	public readonly int SpeedPerC;
	public readonly int AtkSpeedC;

    public EquipGSParamConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out EquipClass); 

			int.TryParse(tables[2],out EquipColor); 

			int.TryParse(tables[3],out EquipStar); 

			int.TryParse(tables[4],out AtkPerC); 

			int.TryParse(tables[5],out DamagePerC); 

			int.TryParse(tables[6],out SuperHitRateC); 

			int.TryParse(tables[7],out SuperHitPerC); 

			int.TryParse(tables[8],out DamReduceC); 

			int.TryParse(tables[9],out MaxHPPerC); 

			int.TryParse(tables[10],out DefPerC); 

			int.TryParse(tables[11],out LuckyHitRateC); 

			int.TryParse(tables[12],out PetDamPerC); 

			int.TryParse(tables[13],out PerLVAtkC); 

			int.TryParse(tables[14],out MissRateC); 

			int.TryParse(tables[15],out HitRateC); 

			int.TryParse(tables[16],out DamBackPerC); 

			int.TryParse(tables[17],out PerLVMaxHPC); 

			int.TryParse(tables[18],out DropEquipPerC); 

			int.TryParse(tables[19],out DropMoneyPerC); 

			int.TryParse(tables[20],out IgnoreDefRateReduceC); 

			int.TryParse(tables[21],out DamChanceDefC); 

			int.TryParse(tables[22],out SuperHitReduceC); 

			int.TryParse(tables[23],out SkillAtkRateC); 

			int.TryParse(tables[24],out SpeedPerC); 

			int.TryParse(tables[25],out AtkSpeedC); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, EquipGSParamConfig> configs = new Dictionary<int, EquipGSParamConfig>();
    public static EquipGSParamConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        EquipGSParamConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new EquipGSParamConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "EquipGSParam.txt";
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

			DebugEx.LogFormat("加载结束EquipGSParamConfig：{0}",   DateTime.Now);
        });
    }

}




