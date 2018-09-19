//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, September 15, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ItemConfig
{

    public readonly int id;
	public readonly int type;
	public readonly int level;
	public readonly string name;
	public readonly int icon;
	public readonly int quality;
	public readonly int starLevel;
	public readonly int price;

    public ItemConfig(string content)
    {
        try
        {
            var tables = content.Split('\t');

            int.TryParse(tables[0],out this.id); 

			int.TryParse(tables[1],out this.type); 

			int.TryParse(tables[2],out this.level);

            this.name = tables[3];

			int.TryParse(tables[4],out this.icon); 

			int.TryParse(tables[5],out this.quality); 

			int.TryParse(tables[6],out this.starLevel); 

			int.TryParse(tables[7],out this.price); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ItemConfig> configs = new Dictionary<int, ItemConfig>();
    public static ItemConfig Get(int id)
    {
        if (configs.ContainsKey(id))
        {
            return configs[id];
        }

        ItemConfig config = null;
        if (rawDatas.ContainsKey(id))
        {
            config = configs[id] = new ItemConfig(rawDatas[id]);
            rawDatas.Remove(id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Item.txt";
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




