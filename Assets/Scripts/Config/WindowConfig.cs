//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, September 03, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class WindowConfig
{

    public readonly int id;
	public readonly bool fullScreen;
	public readonly int depth;
	public readonly bool emptyToClose;

    public WindowConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			var fullScreenTemp = 0;
			int.TryParse(tables[1],out fullScreenTemp); 
			fullScreen=fullScreenTemp!=0;

			int.TryParse(tables[2],out depth); 

			var emptyToCloseTemp = 0;
			int.TryParse(tables[3],out emptyToCloseTemp); 
			emptyToClose=emptyToCloseTemp!=0;
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, WindowConfig> configs = new Dictionary<int, WindowConfig>();
    public static WindowConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        WindowConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new WindowConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Window.txt";
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
        });
    }

}




