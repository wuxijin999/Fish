//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class VipPrivilegeConfig
{

    public readonly int VIPPrivilege;
	public readonly int VIP0;
	public readonly int VIP1;
	public readonly int VIP2;
	public readonly int VIP3;
	public readonly int VIP4;
	public readonly int VIP5;
	public readonly int VIP6;
	public readonly int VIP7;
	public readonly int VIP8;
	public readonly int VIP9;
	public readonly int VIP10;
	public readonly int VIP11;
	public readonly int VIP12;
	public readonly int VIP13;
	public readonly int VIP14;
	public readonly int VIP15;

    public VipPrivilegeConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out VIPPrivilege); 

			int.TryParse(tables[1],out VIP0); 

			int.TryParse(tables[2],out VIP1); 

			int.TryParse(tables[3],out VIP2); 

			int.TryParse(tables[4],out VIP3); 

			int.TryParse(tables[5],out VIP4); 

			int.TryParse(tables[6],out VIP5); 

			int.TryParse(tables[7],out VIP6); 

			int.TryParse(tables[8],out VIP7); 

			int.TryParse(tables[9],out VIP8); 

			int.TryParse(tables[10],out VIP9); 

			int.TryParse(tables[11],out VIP10); 

			int.TryParse(tables[12],out VIP11); 

			int.TryParse(tables[13],out VIP12); 

			int.TryParse(tables[14],out VIP13); 

			int.TryParse(tables[15],out VIP14); 

			int.TryParse(tables[16],out VIP15); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, VipPrivilegeConfig> configs = new Dictionary<int, VipPrivilegeConfig>();
    public static VipPrivilegeConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        VipPrivilegeConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new VipPrivilegeConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "VipPrivilege.txt";
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

			DebugEx.LogFormat("加载结束VipPrivilegeConfig：{0}",   DateTime.Now);
        });
    }

}




