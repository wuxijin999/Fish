//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class MarketConfig
{

    public readonly int type;
	public readonly string name;
	public readonly int limitLv;
	public readonly int[] queryType;
	public readonly int[] ChooseType;
	public readonly string[] ChooseName;
	public readonly int[] ChooseItem1;
	public readonly int[] ChooseItem2;
	public readonly int[] ChooseItem3;
	public readonly string[] itemTypeName;
	public readonly string[] itemTypeIcon;

    public MarketConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out type); 

			name = tables[1];

			int.TryParse(tables[2],out limitLv); 

			string[] queryTypeStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			queryType = new int[queryTypeStringArray.Length];
			for (int i=0;i<queryTypeStringArray.Length;i++)
			{
				 int.TryParse(queryTypeStringArray[i],out queryType[i]);
			}

			string[] ChooseTypeStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ChooseType = new int[ChooseTypeStringArray.Length];
			for (int i=0;i<ChooseTypeStringArray.Length;i++)
			{
				 int.TryParse(ChooseTypeStringArray[i],out ChooseType[i]);
			}

			ChooseName = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			string[] ChooseItem1StringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ChooseItem1 = new int[ChooseItem1StringArray.Length];
			for (int i=0;i<ChooseItem1StringArray.Length;i++)
			{
				 int.TryParse(ChooseItem1StringArray[i],out ChooseItem1[i]);
			}

			string[] ChooseItem2StringArray = tables[7].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ChooseItem2 = new int[ChooseItem2StringArray.Length];
			for (int i=0;i<ChooseItem2StringArray.Length;i++)
			{
				 int.TryParse(ChooseItem2StringArray[i],out ChooseItem2[i]);
			}

			string[] ChooseItem3StringArray = tables[8].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ChooseItem3 = new int[ChooseItem3StringArray.Length];
			for (int i=0;i<ChooseItem3StringArray.Length;i++)
			{
				 int.TryParse(ChooseItem3StringArray[i],out ChooseItem3[i]);
			}

			itemTypeName = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			itemTypeIcon = tables[10].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, MarketConfig> configs = new Dictionary<int, MarketConfig>();
    public static MarketConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        MarketConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new MarketConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Market.txt";
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

			DebugEx.LogFormat("加载结束MarketConfig：{0}",   DateTime.Now);
        });
    }

}




