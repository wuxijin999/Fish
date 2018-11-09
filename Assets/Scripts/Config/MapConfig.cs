//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, November 09, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class MapConfig
{

    public readonly int id;
	public readonly int backGround;
	public readonly int name;
	public readonly int levelMin;
	public readonly int levelMax;
	public readonly int music;
	public readonly int camp;
	public readonly string sceneName;
	public readonly Vector3 bornPoint;

    public MapConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out backGround); 

			int.TryParse(tables[2],out name); 

			int.TryParse(tables[3],out levelMin); 

			int.TryParse(tables[4],out levelMax); 

			int.TryParse(tables[5],out music); 

			int.TryParse(tables[6],out camp); 

			sceneName = tables[7];

			bornPoint=tables[8].Vector3Parse();
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, MapConfig> configs = new Dictionary<int, MapConfig>();
    public static MapConfig Get(int id)
    {   
		if (!inited)
        {
            Debug.Log("MapConfigConfig 还未完成初始化。");
            return null;
        }
		
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        MapConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new MapConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }

	public static bool Has(int id)
    {
        return configs.ContainsKey(id);
    }

	static bool inited = false;
    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
	    inited = false;
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Map.txt";
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

			inited=true;
        });
    }

}




