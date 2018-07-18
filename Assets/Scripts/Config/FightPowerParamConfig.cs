//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class FightPowerParamConfig
{

    public readonly int LV;
	public readonly int Hit;
	public readonly int Miss;
	public readonly int DamagePer;
	public readonly int DamReduce;
	public readonly int IgnoreDefRate;
	public readonly int DamChanceDef;
	public readonly int BleedDamage;
	public readonly int FaintRate;
	public readonly int SuperHitReduce;
	public readonly int LuckyHitRateReduce;
	public readonly int SkillAtkRate;
	public readonly int SkillAtkRateReduce;
	public readonly int DamagePerPVP;
	public readonly int DamagePerPVPReduce;
	public readonly int DamBackPer;
	public readonly int IgnoreDefRateReduce;
	public readonly int FaintDefRate;
	public readonly int AtkSpeedParameter;
	public readonly int LuckyHitParameter;

    public FightPowerParamConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out LV); 

			int.TryParse(tables[1],out Hit); 

			int.TryParse(tables[2],out Miss); 

			int.TryParse(tables[3],out DamagePer); 

			int.TryParse(tables[4],out DamReduce); 

			int.TryParse(tables[5],out IgnoreDefRate); 

			int.TryParse(tables[6],out DamChanceDef); 

			int.TryParse(tables[7],out BleedDamage); 

			int.TryParse(tables[8],out FaintRate); 

			int.TryParse(tables[9],out SuperHitReduce); 

			int.TryParse(tables[10],out LuckyHitRateReduce); 

			int.TryParse(tables[11],out SkillAtkRate); 

			int.TryParse(tables[12],out SkillAtkRateReduce); 

			int.TryParse(tables[13],out DamagePerPVP); 

			int.TryParse(tables[14],out DamagePerPVPReduce); 

			int.TryParse(tables[15],out DamBackPer); 

			int.TryParse(tables[16],out IgnoreDefRateReduce); 

			int.TryParse(tables[17],out FaintDefRate); 

			int.TryParse(tables[18],out AtkSpeedParameter); 

			int.TryParse(tables[19],out LuckyHitParameter); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, FightPowerParamConfig> configs = new Dictionary<int, FightPowerParamConfig>();
    public static FightPowerParamConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        FightPowerParamConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new FightPowerParamConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "FightPowerParam.txt";
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

			DebugEx.LogFormat("加载结束FightPowerParamConfig：{0}",   DateTime.Now);
        });
    }

}




