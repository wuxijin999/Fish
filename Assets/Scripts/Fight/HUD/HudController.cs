using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController
{
    public ActorBase owner { get; private set; }

    Transform headUpPoint;
    HeadUpName headUpName;
    HeadUpGuild headUpGuild;

    public HudController(ActorBase actor)
    {
        this.owner = actor;
    }

    public void Dispose()
    {
        owner = null;
    }

    public void DisplayName(string name, string tile, int level)
    {
        if (headUpName != null)
        {
            HeadUpName.Release(headUpName);
        }

        headUpName = HeadUpName.Get(this.owner.transform, 0.3f, CameraUtil.fightCamera).Display(name, tile, level);
    }

    public void HideName()
    {
        if (headUpName != null)
        {
            HeadUpName.Release(headUpName);
        }

        headUpName = null;
    }

    public void DisplayGuild(string name)
    {
        if (headUpGuild != null)
        {
            HeadUpGuild.Release(headUpGuild);
        }

        headUpGuild = HeadUpGuild.Get(this.owner.transform, 0.3f, CameraUtil.fightCamera).Display(name);
    }

    public void HideGuild()
    {
        if (headUpGuild != null)
        {
            HeadUpGuild.Release(headUpGuild);
        }

        headUpGuild = null;
    }

    public void DisplayDrop()
    {

    }

    public void HideDrop()
    {

    }

}

