using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : Scene
{

    MyPlayer myPlayer;

    public override void OnInitialize()
    {
        PlayerInfo.Instance.SetProperty(PropertyType.Cuirass, 1);

        var playerAvatar = PlayerInfo.Instance.GetPlayerAvatar();
        var prefab = MobAssets.LoadPrefab(playerAvatar.cuirass);
        var model = GameObject.Instantiate(prefab);
        myPlayer = ActorCenter.Instance.Create(ActorType.Player, model.transform) as MyPlayer;

        var mapConfig = MapConfig.Get(SceneLoad.Instance.currentSceneId);
        myPlayer.position = mapConfig.bornPoint;

    }

    public override void OnUnInitialize()
    {
    }


}
