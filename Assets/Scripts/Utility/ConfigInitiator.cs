using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigInitiator
{

    public static void PreInit()
    {
        PriorLanguageConfig.Init();
    }

    public static void Init()
    {
        ItemConfig.Init();
        TestConfig.Init();
        WindowConfig.Init();
        EffectConfig.Init();
        EquipConfig.Init();
		IconConfig.Init();
		LanguageConfig.Init();
		WorldBossConfig.Init();
		NpcConfig.Init();
		MapConfig.Init();
		//初始化结束
    }

}
