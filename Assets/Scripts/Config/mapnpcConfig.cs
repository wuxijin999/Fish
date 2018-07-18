//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class mapnpcConfig
{

    public readonly int RefreshID;
	public readonly int NPCID;
	public readonly string NPCName;
	public readonly int NPCType;
	public readonly int NPCLv;
	public readonly int MapID;
	public readonly int Unknow1;
	public readonly int Unknow2;
	public readonly string HexPosNPCRefreshAreaChaseArea;
	public readonly int PerRefreshNum;
	public readonly int RefreshNum;
	public readonly int NPCFace;
	public readonly int Unknow3;
	public readonly int RefreshTime;
	public readonly int Unknow4;
	public readonly int Unknow5;
	public readonly int Unknow6;
	public readonly int Unknow7;
	public readonly int NPCRoute;
	public readonly int StayMinTime;
	public readonly int StayMaxTime;
	public readonly int RefreshMark;
	public readonly int Unknow9;

    public mapnpcConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out RefreshID); 

			int.TryParse(tables[1],out NPCID); 

			NPCName = tables[2];

			int.TryParse(tables[3],out NPCType); 

			int.TryParse(tables[4],out NPCLv); 

			int.TryParse(tables[5],out MapID); 

			int.TryParse(tables[6],out Unknow1); 

			int.TryParse(tables[7],out Unknow2); 

			HexPosNPCRefreshAreaChaseArea = tables[8];

			int.TryParse(tables[9],out PerRefreshNum); 

			int.TryParse(tables[10],out RefreshNum); 

			int.TryParse(tables[11],out NPCFace); 

			int.TryParse(tables[12],out Unknow3); 

			int.TryParse(tables[13],out RefreshTime); 

			int.TryParse(tables[14],out Unknow4); 

			int.TryParse(tables[15],out Unknow5); 

			int.TryParse(tables[16],out Unknow6); 

			int.TryParse(tables[17],out Unknow7); 

			int.TryParse(tables[18],out NPCRoute); 

			int.TryParse(tables[19],out StayMinTime); 

			int.TryParse(tables[20],out StayMaxTime); 

			int.TryParse(tables[21],out RefreshMark); 

			int.TryParse(tables[22],out Unknow9); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, mapnpcConfig> configs = new Dictionary<int, mapnpcConfig>();
    public static mapnpcConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        mapnpcConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new mapnpcConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "mapnpc.txt";
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

			DebugEx.LogFormat("加载结束mapnpcConfig：{0}",   DateTime.Now);
        });
    }

}




