//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class ViewRoleFuncConfig
{

    public readonly int id;
	public readonly int func;
	public readonly string compareTip;
	public readonly int[] condition;
	public readonly string Icon;

    public ViewRoleFuncConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			int.TryParse(tables[1],out func); 

			compareTip = tables[2];

			string[] conditionStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			condition = new int[conditionStringArray.Length];
			for (int i=0;i<conditionStringArray.Length;i++)
			{
				 int.TryParse(conditionStringArray[i],out condition[i]);
			}

			Icon = tables[4];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, ViewRoleFuncConfig> configs = new Dictionary<int, ViewRoleFuncConfig>();
    public static ViewRoleFuncConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        ViewRoleFuncConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new ViewRoleFuncConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "ViewRoleFunc.txt";
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

			DebugEx.LogFormat("加载结束ViewRoleFuncConfig：{0}",   DateTime.Now);
        });
    }

}




