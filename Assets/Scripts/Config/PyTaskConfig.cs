//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class PyTaskConfig
{

    public readonly string id;
	public readonly string name;
	public readonly int type;
	public readonly int npcId;
	public readonly int lv;
	public readonly int colorLV;
	public readonly int isCanDelete;
	public readonly string descList;
	public readonly string rewardList;
	public readonly string lightList;
	public readonly string infoList;
	public readonly string guideDesc;
	public readonly int isShowGuideReward;
	public readonly string szDayCount;

    public PyTaskConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            id = tables[0];

			name = tables[1];

			int.TryParse(tables[2],out type); 

			int.TryParse(tables[3],out npcId); 

			int.TryParse(tables[4],out lv); 

			int.TryParse(tables[5],out colorLV); 

			int.TryParse(tables[6],out isCanDelete); 

			descList = tables[7];

			rewardList = tables[8];

			lightList = tables[9];

			infoList = tables[10];

			guideDesc = tables[11];

			int.TryParse(tables[12],out isShowGuideReward); 

			szDayCount = tables[13];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, PyTaskConfig> configs = new Dictionary<int, PyTaskConfig>();
    public static PyTaskConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        PyTaskConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new PyTaskConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "PyTask.txt";
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

			DebugEx.LogFormat("加载结束PyTaskConfig：{0}",   DateTime.Now);
        });
    }

}




