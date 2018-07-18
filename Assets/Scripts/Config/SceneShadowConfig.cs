//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class SceneShadowConfig
{

    public readonly string SceneName;
	public readonly Vector3 Rotation;
	public readonly float Intensity;

    public SceneShadowConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            SceneName = tables[0];

			Rotation=tables[1].Vector3Parse();

			float.TryParse(tables[2],out Intensity); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, SceneShadowConfig> configs = new Dictionary<int, SceneShadowConfig>();
    public static SceneShadowConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        SceneShadowConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new SceneShadowConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "SceneShadow.txt";
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

			DebugEx.LogFormat("加载结束SceneShadowConfig：{0}",   DateTime.Now);
        });
    }

}




