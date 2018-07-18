//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;

public class ItemConfig
{

    public readonly int NPCID;
    public readonly int FloorNum;
    public readonly int MonsterType;
    public readonly int[] RareItemID;
    public readonly string PortraitID;

    public ItemConfig(string _content)
    {
        var tables = _content.Split('\t');
        int.TryParse(tables[0], out NPCID);

        int.TryParse(tables[1], out FloorNum);

        int.TryParse(tables[2], out MonsterType);

        string[] RareItemIDStringArray = tables[3].Trim().Split(StringUtility.splitSeparator, StringSplitOptions.RemoveEmptyEntries);
        RareItemID = new int[RareItemIDStringArray.Length];
        for (int i = 0; i < RareItemIDStringArray.Length; i++)
        {
            int.TryParse(RareItemIDStringArray[i], out RareItemID[i]);
        }

        PortraitID = tables[4];
    }

    static Dictionary<int, ItemConfig> configs = new Dictionary<int, ItemConfig>();
    public static ItemConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ItemConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ItemConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ItemConfig.txt";
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




