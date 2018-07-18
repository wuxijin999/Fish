//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ContinueSignInConfig
{

    public readonly int ContineDay;
	public readonly int[] ItemID;
	public readonly int IsBind;
	public readonly int[] ItemNum;
	public readonly int[] JobItemList;

    public ContinueSignInConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ContineDay); 

			string[] ItemIDStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ItemID = new int[ItemIDStringArray.Length];
			for (int i=0;i<ItemIDStringArray.Length;i++)
			{
				 int.TryParse(ItemIDStringArray[i],out ItemID[i]);
			}

			int.TryParse(tables[2],out IsBind); 

			string[] ItemNumStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ItemNum = new int[ItemNumStringArray.Length];
			for (int i=0;i<ItemNumStringArray.Length;i++)
			{
				 int.TryParse(ItemNumStringArray[i],out ItemNum[i]);
			}

			string[] JobItemListStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			JobItemList = new int[JobItemListStringArray.Length];
			for (int i=0;i<JobItemListStringArray.Length;i++)
			{
				 int.TryParse(JobItemListStringArray[i],out JobItemList[i]);
			}
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ContinueSignInConfig> configs = new Dictionary<int, ContinueSignInConfig>();
    public static ContinueSignInConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ContinueSignInConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ContinueSignInConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ContinueSignIn.txt";
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

			DebugEx.LogFormat("加载结束ContinueSignInConfig：{0}",   DateTime.Now);
        });
    }

}




