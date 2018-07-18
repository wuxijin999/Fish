//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class AppointItemConfig
{

    public readonly int ID;
	public readonly int SuiteLv;
	public readonly int CancelUseLimit;
	public readonly int[] LegendAttrID;
	public readonly int[] LegendAttrValue;
	public readonly int[] OutOfPrintAttr;
	public readonly int[] OutOfPrintAttrValue;

    public AppointItemConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out SuiteLv); 

			int.TryParse(tables[2],out CancelUseLimit); 

			string[] LegendAttrIDStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			LegendAttrID = new int[LegendAttrIDStringArray.Length];
			for (int i=0;i<LegendAttrIDStringArray.Length;i++)
			{
				 int.TryParse(LegendAttrIDStringArray[i],out LegendAttrID[i]);
			}

			string[] LegendAttrValueStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			LegendAttrValue = new int[LegendAttrValueStringArray.Length];
			for (int i=0;i<LegendAttrValueStringArray.Length;i++)
			{
				 int.TryParse(LegendAttrValueStringArray[i],out LegendAttrValue[i]);
			}

			string[] OutOfPrintAttrStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			OutOfPrintAttr = new int[OutOfPrintAttrStringArray.Length];
			for (int i=0;i<OutOfPrintAttrStringArray.Length;i++)
			{
				 int.TryParse(OutOfPrintAttrStringArray[i],out OutOfPrintAttr[i]);
			}

			string[] OutOfPrintAttrValueStringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			OutOfPrintAttrValue = new int[OutOfPrintAttrValueStringArray.Length];
			for (int i=0;i<OutOfPrintAttrValueStringArray.Length;i++)
			{
				 int.TryParse(OutOfPrintAttrValueStringArray[i],out OutOfPrintAttrValue[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, AppointItemConfig> configs = new Dictionary<int, AppointItemConfig>();
    public static AppointItemConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        AppointItemConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new AppointItemConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "AppointItem.txt";
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

			DebugEx.LogFormat("加载结束AppointItemConfig：{0}",   DateTime.Now);
        });
    }

}




