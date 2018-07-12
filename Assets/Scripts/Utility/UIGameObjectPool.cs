using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameObjectPool
{
    private List<GameObject> m_FreeList = new List<GameObject>();
    private List<GameObject> m_ActiveList = new List<GameObject>();
    private GameObject m_Prefab;

    public readonly GameObject root;
    public readonly int instanceId = 0;
    string name;

    public UIGameObjectPool(int _instanceId, GameObject _prefab)
    {
        root = new GameObject(StringUtility.Contact("UIPool_", _instanceId));
        Object.DontDestroyOnLoad(root);
        root.transform.position = UIGameObjectPoolUtility.HIDE_POINT;

        instanceId = _instanceId;
        m_Prefab = _prefab;
        name = _prefab.name;
    }

    public GameObject Get()
    {
        GameObject instance = null;
        if (m_FreeList.Count == 0)
        {
            instance = Object.Instantiate(m_Prefab);
            instance.name = name;
        }
        else
        {
            instance = m_FreeList[0];
            m_FreeList.RemoveAt(0);
        }

        m_ActiveList.Add(instance);
        return instance;
    }

    public void Release(GameObject instance)
    {
        if (m_ActiveList.Contains(instance))
        {
            m_ActiveList.Remove(instance);
        }
        else
        {
            DebugEx.LogWarningFormat("回收的对象 {0} 并不是从池里取得的...", instance.name);
        }

        instance.transform.SetParent(root.transform);
        if (!m_FreeList.Contains(instance))
        {
            m_FreeList.Add(instance);
        }
    }

    public void Clear()
    {
        foreach (var item in m_FreeList)
        {
            Object.Destroy(item);
        }

        foreach (var item in m_ActiveList)
        {
            Object.Destroy(item);
        }

        m_FreeList.Clear();
        m_ActiveList.Clear();
    }

    public void Destroy()
    {
        Clear();

        m_Prefab = null;
        m_FreeList = null;
        m_ActiveList = null;
        GameObject.Destroy(root);
    }


#if UNITY_EDITOR
    public List<GameObject> GetFreeList()
    {
        return new List<GameObject>(m_FreeList);
    }

    public List<GameObject> GetActiveList()
    {
        return new List<GameObject>(m_ActiveList);
    }

#endif

}
