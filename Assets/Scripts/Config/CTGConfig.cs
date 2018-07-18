//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class CTGConfig
{

    public readonly int RecordID;
	public readonly string Title;
	public readonly string OrderInfo;
	public readonly string AppId;
	public readonly int DailyBuyCount;
	public readonly float PayRMBNum;
	public readonly int GainGold;
	public readonly int GainGoldPaper;
	public readonly int FirstGoldPaperPrize;
	public readonly string GainItemList;
	public readonly string Icon;
	public readonly int PayType;

    public CTGConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out RecordID); 

			Title = tables[1];

			OrderInfo = tables[2];

			AppId = tables[3];

			int.TryParse(tables[4],out DailyBuyCount); 

			float.TryParse(tables[5],out PayRMBNum); 

			int.TryParse(tables[6],out GainGold); 

			int.TryParse(tables[7],out GainGoldPaper); 

			int.TryParse(tables[8],out FirstGoldPaperPrize); 

			GainItemList = tables[9];

			Icon = tables[10];

			int.TryParse(tables[11],out PayType); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, CTGConfig> configs = new Dictionary<int, CTGConfig>();
    public static CTGConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        CTGConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new CTGConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "CTG.txt";
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

			DebugEx.LogFormat("加载结束CTGConfig：{0}",   DateTime.Now);
        });
    }

}




