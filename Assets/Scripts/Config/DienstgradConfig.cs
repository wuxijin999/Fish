//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DienstgradConfig
{

    public readonly int ID;
	public readonly string Name;
	public readonly int Type;
	public readonly int Prescription;
	public readonly string OutTimeDesc;
	public readonly int[] LightType;
	public readonly int[] LightAttribute;
	public readonly string Image;
	public readonly string Content;
	public readonly int[] Skills;
	public readonly int gotoId;
	public readonly int missionId;

    public DienstgradConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			Name = tables[1];

			int.TryParse(tables[2],out Type); 

			int.TryParse(tables[3],out Prescription); 

			OutTimeDesc = tables[4];

			string[] LightTypeStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			LightType = new int[LightTypeStringArray.Length];
			for (int i=0;i<LightTypeStringArray.Length;i++)
			{
				 int.TryParse(LightTypeStringArray[i],out LightType[i]);
			}

			string[] LightAttributeStringArray = tables[6].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			LightAttribute = new int[LightAttributeStringArray.Length];
			for (int i=0;i<LightAttributeStringArray.Length;i++)
			{
				 int.TryParse(LightAttributeStringArray[i],out LightAttribute[i]);
			}

			Image = tables[7];

			Content = tables[8];

			string[] SkillsStringArray = tables[9].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			Skills = new int[SkillsStringArray.Length];
			for (int i=0;i<SkillsStringArray.Length;i++)
			{
				 int.TryParse(SkillsStringArray[i],out Skills[i]);
			}

			int.TryParse(tables[10],out gotoId); 

			int.TryParse(tables[11],out missionId); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DienstgradConfig> configs = new Dictionary<int, DienstgradConfig>();
    public static DienstgradConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DienstgradConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DienstgradConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Dienstgrad.txt";
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

			DebugEx.LogFormat("加载结束DienstgradConfig：{0}",   DateTime.Now);
        });
    }

}




