//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RealmConfig
{

    public readonly int Lv;
	public readonly string Name;
	public readonly int IsBigRealm;
	public readonly int NeedPoint;
	public readonly int NeedGood;
	public readonly int NeedNum;
	public readonly string NeedActiveTreasure;
	public readonly int[] AddAttrType;
	public readonly int[] AddAttrNum;
	public readonly int BossID;
	public readonly string Img;
	public readonly string SitTime;
	public readonly int Quality;
	public readonly int FightPower;

    public RealmConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out Lv); 

			Name = tables[1];

			int.TryParse(tables[2],out IsBigRealm); 

			int.TryParse(tables[3],out NeedPoint); 

			int.TryParse(tables[4],out NeedGood); 

			int.TryParse(tables[5],out NeedNum); 

			NeedActiveTreasure = tables[6];

			string[] AddAttrTypeStringArray = tables[7].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AddAttrType = new int[AddAttrTypeStringArray.Length];
			for (int i=0;i<AddAttrTypeStringArray.Length;i++)
			{
				 int.TryParse(AddAttrTypeStringArray[i],out AddAttrType[i]);
			}

			string[] AddAttrNumStringArray = tables[8].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AddAttrNum = new int[AddAttrNumStringArray.Length];
			for (int i=0;i<AddAttrNumStringArray.Length;i++)
			{
				 int.TryParse(AddAttrNumStringArray[i],out AddAttrNum[i]);
			}

			int.TryParse(tables[9],out BossID); 

			Img = tables[10];

			SitTime = tables[11];

			int.TryParse(tables[12],out Quality); 

			int.TryParse(tables[13],out FightPower); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RealmConfig> configs = new Dictionary<int, RealmConfig>();
    public static RealmConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RealmConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RealmConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Realm.txt";
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

			DebugEx.LogFormat("加载结束RealmConfig：{0}",   DateTime.Now);
        });
    }

}




