using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.U2D;
using System;

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

    }

}

