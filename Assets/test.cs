using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.U2D;
using System;
using System.Timers;
using System.Threading;

public class test : MonoBehaviour
{


    [SerializeField] SpriteAtlas m_Atlas;

    private void OnEnable()
    {
        var sprite = m_Atlas.GetSprite("Arrows_a");
        Sprite[] sprites = null;
        var count = m_Atlas.GetSprites(sprites, "Arrows_a");

        Debug.Log(m_Atlas.tag);
        Debug.Log(m_Atlas.spriteCount);
        Debug.Log(sprite.name);
        Debug.Log(count);

        ThreadPool.QueueUserWorkItem(aaa);
        ThreadPool.QueueUserWorkItem(bbb);
        ThreadPool.QueueUserWorkItem(ccc);
    }

    private void aaa(object sender, System.Timers.ElapsedEventArgs e)
    {
        DebugEx.Log("现在是 10000 结束");
    }

    private void aaa(object _aaa)
    {
        DebugEx.Log("aaa");
    }

    private void bbb(object _aaa)
    {
        Thread.Sleep(1000);
        DebugEx.Log("bbb");
    }

    private void ccc(object _aaa)
    {
        Thread.Sleep(2000);
        DebugEx.Log("ccc");
    }

}

