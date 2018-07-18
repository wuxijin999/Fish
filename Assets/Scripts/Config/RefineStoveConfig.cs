//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class RefineStoveConfig
{

    public readonly int LV;
	public readonly int Exp;
	public readonly int[] AttrID;
	public readonly int[] AttrValue;

    public RefineStoveConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out LV); 

			int.TryParse(tables[1],out Exp); 

			string[] AttrIDStringArray = tables[2].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AttrID = new int[AttrIDStringArray.Length];
			for (int i=0;i<AttrIDStringArray.Length;i++)
			{
				 int.TryParse(AttrIDStringArray[i],out AttrID[i]);
			}

			string[] AttrValueStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			AttrValue = new int[AttrValueStringArray.Length];
			for (int i=0;i<AttrValueStringArray.Length;i++)
			{
				 int.TryParse(AttrValueStringArray[i],out AttrValue[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, RefineStoveConfig> configs = new Dictionary<int, RefineStoveConfig>();
    public static RefineStoveConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        RefineStoveConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new RefineStoveConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "RefineStove.txt";
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

			DebugEx.LogFormat("加载结束RefineStoveConfig：{0}",   DateTime.Now);
        });
    }

}




