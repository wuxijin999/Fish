using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpGuild : HUDBase
{

    [SerializeField] TextEx m_GuildName;

    public static HeadUpGuild Get(Transform target, float offsetY, Camera camera)
    {
        var headGuild = HeadUpGuildPool.Get();
        headGuild.Follow(target, Vector3.zero.SetY(offsetY), camera);
        headGuild.transform.SetParentEx(UIRoot.windowRoot);
        return headGuild;
    }

    public static void Release(HeadUpGuild headUpGuild)
    {
        if (headUpGuild != null)
        {
            headUpGuild.Dispose();
            HeadUpGuildPool.Release(headUpGuild);
        }
    }

    public HeadUpGuild Display(string name)
    {
        m_GuildName.SetText(name);
        return this;
    }

}
