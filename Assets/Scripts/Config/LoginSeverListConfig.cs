//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class LoginSeverListConfig
{

    public readonly int ID;
	public readonly string ip;
	public readonly string pagePort;
	public readonly int gatePort;
	public readonly string serverName;

    public LoginSeverListConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			ip = tables[1];

			pagePort = tables[2];

			int.TryParse(tables[3],out gatePort); 

			serverName = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, LoginSeverListConfig> configs = new Dictionary<int, LoginSeverListConfig>();
    public static LoginSeverListConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        LoginSeverListConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new LoginSeverListConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "LoginSeverList.txt";
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

			DebugEx.LogFormat("加载结束LoginSeverListConfig：{0}",   DateTime.Now);
        });
    }

}




