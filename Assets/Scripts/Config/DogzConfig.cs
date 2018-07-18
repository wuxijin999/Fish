//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class DogzConfig
{

    public readonly int id;
	public readonly string name;
	public readonly string HeadIcon;
	public readonly int[] propertyTypes;
	public readonly int[] propertyValues;
	public readonly int[] skills;
	public readonly string EquipLimit;

    public DogzConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out id); 

			name = tables[1];

			HeadIcon = tables[2];

			string[] propertyTypesStringArray = tables[3].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			propertyTypes = new int[propertyTypesStringArray.Length];
			for (int i=0;i<propertyTypesStringArray.Length;i++)
			{
				 int.TryParse(propertyTypesStringArray[i],out propertyTypes[i]);
			}

			string[] propertyValuesStringArray = tables[4].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			propertyValues = new int[propertyValuesStringArray.Length];
			for (int i=0;i<propertyValuesStringArray.Length;i++)
			{
				 int.TryParse(propertyValuesStringArray[i],out propertyValues[i]);
			}

			string[] skillsStringArray = tables[5].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			skills = new int[skillsStringArray.Length];
			for (int i=0;i<skillsStringArray.Length;i++)
			{
				 int.TryParse(skillsStringArray[i],out skills[i]);
			}

			EquipLimit = tables[6];
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, DogzConfig> configs = new Dictionary<int, DogzConfig>();
    public static DogzConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        DogzConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new DogzConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Dogz.txt";
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

			DebugEx.LogFormat("加载结束DogzConfig：{0}",   DateTime.Now);
        });
    }

}




