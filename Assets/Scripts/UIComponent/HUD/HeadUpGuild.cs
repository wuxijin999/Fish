using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpGuild : HUDBase
{

    [SerializeField] TextEx m_GuildName;

    public static HeadUpGuild Get(Transform target, float offsetY, Camera camera)
    {
        var headUpName = HeadUpGuildPool.Get();
        headUpName.camera = camera;
        headUpName.target = target;
        headUpName.offset = new Vector3(0, offsetY, 0);
        headUpName.transform.SetParentEx(UIRoot.windowRoot);
        headUpName.SyncPosition(true);
        return headUpName;
    }

    public static void Release(HeadUpGuild headUpGuild)
    {
        if (headUpGuild != null)
        {
            HeadUpGuildPool.Release(headUpGuild);
        }
    }

    public HeadUpGuild Display(string name)
    {
        m_GuildName.SetText(name);
        SyncPosition(true);
        return this;
    }

}
