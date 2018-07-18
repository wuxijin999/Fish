//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class MapResourcesConfig
{

    public readonly int ID;
	public readonly int DataID;
	public readonly int LineID;
	public readonly string Name;
	public readonly string BigMap;
	public readonly Vector3 MapScale;
	public readonly float MapShowScale;
	public readonly string MapResources;
	public readonly Vector3 MapOffset;
	public readonly int[] Effect;
	public readonly int Music;
	public readonly string[] LoadingBG;
	public readonly string LoadName;
	public readonly string LoadDescription;
	public readonly int ShowBOSSTime;

    public MapResourcesConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out DataID); 

			int.TryParse(tables[2],out LineID); 

			Name = tables[3];

			BigMap = tables[4];

			MapScale=tables[5].Vector3Parse();

			float.TryParse(tables[6],out MapShowScale); 

			MapResources = tables[7];

			MapOffset=tables[8].Vector3Parse();

			string[] EffectStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Effect = new int[EffectStringArray.Length];
			for (int i=0;i<EffectStringArray.Length;i++)
			{
				 int.TryParse(EffectStringArray[i],out Effect[i]);
			}

			int.TryParse(tables[10],out Music); 

			LoadingBG = tables[11].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);

			LoadName = tables[12];

			LoadDescription = tables[13];

			int.TryParse(tables[14],out ShowBOSSTime); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, MapResourcesConfig> configs = new Dictionary<int, MapResourcesConfig>();
    public static MapResourcesConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        MapResourcesConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new MapResourcesConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "MapResources.txt";
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

			DebugEx.LogFormat("加载结束MapResourcesConfig：{0}",   DateTime.Now);
        });
    }

}




