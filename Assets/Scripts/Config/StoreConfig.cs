//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, July 18, 2018
//--------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using UnityEngine;

public partial class StoreConfig
{

    public readonly int ID;
	public readonly int ShopType;
	public readonly int ShopSort;
	public readonly int ItemID;
	public readonly int ItemCnt;
	public readonly int IsBind;
	public readonly string ItemListEx;
	public readonly int RefreshType;
	public readonly int[] VIPLV;
	public readonly int LV;
	public readonly int[] PurchaseNumber;
	public readonly int MoneyType;
	public readonly int MoneyNumber;
	public readonly int LimitValue;
	public readonly string SalesStatus;
	public readonly int TheOnlyShop;

    public StoreConfig(string _content)
    {
        try
        {
            var tables = _content.Split('\t');

            int.TryParse(tables[0],out ID); 

			int.TryParse(tables[1],out ShopType); 

			int.TryParse(tables[2],out ShopSort); 

			int.TryParse(tables[3],out ItemID); 

			int.TryParse(tables[4],out ItemCnt); 

			int.TryParse(tables[5],out IsBind); 

			ItemListEx = tables[6];

			int.TryParse(tables[7],out RefreshType); 

			string[] VIPLVStringArray = tables[8].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			VIPLV = new int[VIPLVStringArray.Length];
			for (int i=0;i<VIPLVStringArray.Length;i++)
			{
				 int.TryParse(VIPLVStringArray[i],out VIPLV[i]);
			}

			int.TryParse(tables[9],out LV); 

			string[] PurchaseNumberStringArray = tables[10].Trim().Split(StringUtility.splitSeparator,StringSplitOptions.RemoveEmptyEntries);
			PurchaseNumber = new int[PurchaseNumberStringArray.Length];
			for (int i=0;i<PurchaseNumberStringArray.Length;i++)
			{
				 int.TryParse(PurchaseNumberStringArray[i],out PurchaseNumber[i]);
			}

			int.TryParse(tables[11],out MoneyType); 

			int.TryParse(tables[12],out MoneyNumber); 

			int.TryParse(tables[13],out LimitValue); 

			SalesStatus = tables[14];

			int.TryParse(tables[15],out TheOnlyShop); 
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }
    }

    static Dictionary<int, StoreConfig> configs = new Dictionary<int, StoreConfig>();
    public static StoreConfig Get(int _id)
    {
        if (configs.ContainsKey(_id))
        {
            return configs[_id];
        }

        StoreConfig config = null;
        if (rawDatas.ContainsKey(_id))
        {
            config = configs[_id] = new StoreConfig(rawDatas[_id]);
            rawDatas.Remove(_id);
        }

        return config;
    }


    protected static Dictionary<int, string> rawDatas = null;
    public static void Init()
    {
        var path = AssetPath.CONFIG_ROOT_PATH + Path.DirectorySeparatorChar + "Store.txt";
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

			DebugEx.LogFormat("加载结束StoreConfig：{0}",   DateTime.Now);
        });
    }

}




