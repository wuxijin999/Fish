using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : Scene
{

    MyPlayer myPlayer;

    public override void OnInitialize()
    {
        PlayerInfo.Instance.SetProperty(PropertyType.Cuirass, 2);

        var playerAvatar = PlayerInfo.Instance.GetPlayerAvatar();
        var prefab = MobAssets.LoadPrefab(playerAvatar.cuirass);
        var model = GameObject.Instantiate(prefab);
        myPlayer = ActorCenter.Instance.Create(ActorType.Player, model.transform) as MyPlayer;

        var mapConfig = MapConfig.Get(SceneLoad.Instance.currentSceneId);
        myPlayer.position = mapConfig.bornPoint;
        myPlayer.propertyController.SetProperty(FightProperty.MoveSpeed, 20000);

        SkillCast.Instance.SetSkill(1, 10001);
        SkillCast.Instance.SetSkill(2, 10001);
        SkillCast.Instance.SetSkill(3, 10001);
        SkillCast.Instance.SetSkill(4, 10001);
        SkillCast.Instance.SetSkill(5, 10001);
    }

    public override void OnUnInitialize()
    {
    }


}
