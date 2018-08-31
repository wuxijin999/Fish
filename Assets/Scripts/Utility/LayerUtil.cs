using UnityEngine;
using System.Collections;
using System;

public static class LayerUtil
{
    public static readonly int DefaultLayer = LayerMask.NameToLayer("Default");
    public static readonly int DefaultMask = 1 << DefaultLayer;

    public static readonly int GroundLayer = LayerMask.NameToLayer("Ground");
    public static readonly int GroundMask = 1 << GroundLayer;

    public static readonly int Wall = LayerMask.NameToLayer("Wall");
    public static readonly int WallMask = 1 << Wall;

    public static readonly int UILayer = LayerMask.NameToLayer("UI");
    public static readonly int UIMask = 1 << UILayer;

    public static readonly int DevisableUI = LayerMask.NameToLayer("DevisableUI");
    public static readonly int DevisableUIMask = 1 << DevisableUI;

    public static readonly int UIEffectLayer = LayerMask.NameToLayer("UIEffect");
    public static readonly int UIEffectMask = 1 << UILayer;

    public static readonly int HUDLayer = LayerMask.NameToLayer("HUD");
    public static readonly int HUDMask = 1 << HUDLayer;

    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int PlayerMask = 1 << Player;

    public static readonly int Monster = LayerMask.NameToLayer("Monster");
    public static readonly int MonsterMask = 1 << Monster;

    public static readonly int TransparentFX = LayerMask.NameToLayer("TransparentFX");
    public static readonly int TransparentFXMask = 1 << TransparentFX;

    public static readonly int Hero = LayerMask.NameToLayer("Hero");
    public static readonly int HeroMask = 1 << Hero;

    public static readonly int MapTrigger = LayerMask.NameToLayer("MapTrigger");
    public static readonly int MapTriggerMask = 1 << MapTrigger;

    public static readonly int Walkble = LayerMask.NameToLayer("WalkbleLayer");
    public static readonly int WalkbleMask = 1 << Walkble;

    public static readonly int BossShow = LayerMask.NameToLayer("BossShow");
    public static readonly int BossShowMask = 1 << BossShow;

    public static readonly int BattleEffect = LayerMask.NameToLayer("BattleEffect");
    public static readonly int BattleEffectMask = 1 << BattleEffect;

    public static readonly int Hide = LayerMask.NameToLayer("Hide");
    public static readonly int HideMask = 1 << Hide;

    public static readonly int MaskShow = LayerMask.NameToLayer("MaskShow");
    public static readonly int MaskShowMask = 1 << MaskShow;


    public static void SetLayer(this GameObject _gameObject, int _layer, bool _recursive)
    {
        if (_gameObject == null)
        {
            return;
        }

        _gameObject.layer = _layer;

        if (_recursive && _gameObject.transform.childCount > 0)
        {
            var childCount = _gameObject.transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = _gameObject.transform.GetChild(i);
                SetLayer(child.gameObject, _layer, true);
            }
        }

    }

}
