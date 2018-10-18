using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private List<GameObject> m_FreeList = new List<GameObject>();
    private List<GameObject> m_ActiveList = new List<GameObject>();
    private GameObject m_Prefab;

    public readonly GameObject root;
    public readonly int instanceId = 0;
    string name;

    public GameObjectPool(int _instanceId, GameObject _prefab)
    {
        this.root = new GameObject(StringUtil.Contact("UIPool_", _instanceId));
        Object.DontDestroyOnLoad(this.root);
        this.root.transform.position = GameObjectPoolUtil.HIDE_POINT;

        this.instanceId = _instanceId;
        this.m_Prefab = _prefab;
        this.name = _prefab.name;
    }

    public GameObject Get()
    {
        GameObject instance = null;
        if (this.m_FreeList.Count == 0)
        {
            instance = Object.Instantiate(this.m_Prefab);
            instance.name = this.name;
        }
        else
        {
            instance = this.m_FreeList[0];
            this.m_FreeList.RemoveAt(0);
        }

        this.m_ActiveList.Add(instance);
        return instance;
    }

    public void Release(GameObject instance)
    {
        if (this.m_ActiveList.Contains(instance))
        {
            this.m_ActiveList.Remove(instance);
        }
        else
        {
            DebugEx.LogWarningFormat("回收的对象 {0} 并不是从池里取得的...", instance.name);
        }

        instance.transform.SetParent(this.root.transform);
        this.m_FreeList.AddEx(instance);
    }

    public void ReleaseAll()
    {
        foreach (var item in m_ActiveList)
        {
            Release(item);
        }
    }

    public void Clear()
    {
        foreach (var item in this.m_FreeList)
        {
            Object.Destroy(item);
        }

        foreach (var item in this.m_ActiveList)
        {
            Object.Destroy(item);
        }

        this.m_FreeList.Clear();
        this.m_ActiveList.Clear();
    }

    public void Destroy()
    {
        Clear();

        this.m_Prefab = null;
        this.m_FreeList = null;
        this.m_ActiveList = null;
        GameObject.Destroy(this.root);
    }


#if UNITY_EDITOR
    public List<GameObject> GetFreeList()
    {
        return new List<GameObject>(this.m_FreeList);
    }

    public List<GameObject> GetActiveList()
    {
        return new List<GameObject>(this.m_ActiveList);
    }

#endif

}
