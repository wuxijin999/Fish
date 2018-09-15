using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigInitiator
{

    public static void Init()
    {
        ItemConfig.Init();
        TestConfig.Init();
        WindowConfig.Init();
        EffectConfig.Init();
        EquipConfig.Init();
		IconConfig.Init();
		LanguageConfig.Init();
		//初始化结束
    }

}
