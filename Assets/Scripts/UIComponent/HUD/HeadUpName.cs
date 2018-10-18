using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpName : HUDBase
{
    [SerializeField] TextEx m_NameBehaviour;
    [SerializeField] TextEx m_LevelBehaviour;
    [SerializeField] TextEx m_TitleBehaviour;

    public static HeadUpName Get(Transform target, float offsetY, Camera camera)
    {
        var headUpName = HeadUpNamePool.Get();
        headUpName.Follow(target, Vector3.zero.SetY(offsetY), camera);
        headUpName.transform.SetParentEx(UIRoot.hudRoot.headUpBar.transform);
        return headUpName;
    }

    public static void Release(HeadUpName headUpName)
    {
        if (headUpName != null)
        {
            headUpName.Dispose();
            HeadUpNamePool.Release(headUpName);
        }
    }

    public HeadUpName Display(string name, string title, int level)
    {
        if (string.IsNullOrEmpty(name))
        {
            m_NameBehaviour.SetActive(false);
        }
        else
        {
            m_NameBehaviour.SetActive(true);
            m_NameBehaviour.SetText(name);
        }

        if (string.IsNullOrEmpty(title))
        {
            m_TitleBehaviour.SetActive(false);
        }
        else
        {
            m_TitleBehaviour.SetActive(true);
            m_TitleBehaviour.SetText(title);
        }

        if (level <= 0)
        {
            m_LevelBehaviour.SetActive(false);
        }
        else
        {
            m_LevelBehaviour.SetActive(true);
            m_LevelBehaviour.SetText(level);
        }

        return this;
    }



}
