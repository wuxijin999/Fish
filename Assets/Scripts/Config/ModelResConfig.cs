//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ModelResConfig
{

    public readonly int ID;
	public readonly string Name;
	public readonly int Type;
	public readonly string ResourcesName;
	public readonly string BindPoint;
	public readonly string EffFileName;
	public readonly Vector3 UIOffset;
	public readonly Vector3 UIRotation;
	public readonly float UIScale;

    public ModelResConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			Name = tables[1];

			int.TryParse(tables[2],out Type); 

			ResourcesName = tables[3];

			BindPoint = tables[4];

			EffFileName = tables[5];

			UIOffset=tables[6].Vector3Parse();

			UIRotation=tables[7].Vector3Parse();

			float.TryParse(tables[8],out UIScale); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ModelResConfig> configs = new Dictionary<int, ModelResConfig>();
    public static ModelResConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ModelResConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ModelResConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ModelRes.txt";
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

			DebugEx.LogFormat("加载结束ModelResConfig：{0}",   DateTime.Now);
        });
    }

}




