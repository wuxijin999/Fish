//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class LoginAdConfig
{

    public readonly int id;
	public readonly string image;
	public readonly int jump;
	public readonly int[] condition;
	public readonly string gotoIcon;

    public LoginAdConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			image = tables[1];

			int.TryParse(tables[2],out jump); 

			string[] conditionStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			condition = new int[conditionStringArray.Length];
			for (int i=0;i<conditionStringArray.Length;i++)
			{
				 int.TryParse(conditionStringArray[i],out condition[i]);
			}

			gotoIcon = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, LoginAdConfig> configs = new Dictionary<int, LoginAdConfig>();
    public static LoginAdConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        LoginAdConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new LoginAdConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "LoginAd.txt";
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

			DebugEx.LogFormat("加载结束LoginAdConfig：{0}",   DateTime.Now);
        });
    }

}




