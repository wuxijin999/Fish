//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ItemPlusConfig
{

    public readonly int id;
	public readonly int type;
	public readonly int level;
	public readonly string attType;
	public readonly string attValue;
	public readonly int costCount;
	public readonly int getExp;
	public readonly int upExpNeed;
	public readonly int upExpTotal;

    public ItemPlusConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out type); 

			int.TryParse(tables[2],out level); 

			attType = tables[3];

			attValue = tables[4];

			int.TryParse(tables[5],out costCount); 

			int.TryParse(tables[6],out getExp); 

			int.TryParse(tables[7],out upExpNeed); 

			int.TryParse(tables[8],out upExpTotal); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ItemPlusConfig> configs = new Dictionary<int, ItemPlusConfig>();
    public static ItemPlusConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ItemPlusConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ItemPlusConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ItemPlus.txt";
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

			DebugEx.LogFormat("加载结束ItemPlusConfig：{0}",   DateTime.Now);
        });
    }

}




