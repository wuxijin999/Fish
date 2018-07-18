//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class SignInConfig
{

    public readonly int RewardID;
	public readonly int[] ItemID;
	public readonly int IsBind;
	public readonly int VipLv;
	public readonly int[] OrdinaryNum;
	public readonly int VipMultiple;

    public SignInConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out RewardID); 

			string[] ItemIDStringArray = tables[1].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			ItemID = new int[ItemIDStringArray.Length];
			for (int i=0;i<ItemIDStringArray.Length;i++)
			{
				 int.TryParse(ItemIDStringArray[i],out ItemID[i]);
			}

			int.TryParse(tables[2],out IsBind); 

			int.TryParse(tables[3],out VipLv); 

			string[] OrdinaryNumStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			OrdinaryNum = new int[OrdinaryNumStringArray.Length];
			for (int i=0;i<OrdinaryNumStringArray.Length;i++)
			{
				 int.TryParse(OrdinaryNumStringArray[i],out OrdinaryNum[i]);
			}

			int.TryParse(tables[5],out VipMultiple); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, SignInConfig> configs = new Dictionary<int, SignInConfig>();
    public static SignInConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        SignInConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new SignInConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "SignIn.txt";
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

			DebugEx.LogFormat("加载结束SignInConfig：{0}",   DateTime.Now);
        });
    }

}




