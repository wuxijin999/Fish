//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, August 31, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ImageEx : Image
{

    bool m_Gray = false;
    public bool gray
    {
        get
        {
            return this.m_Gray;
        }
        set
        {
            this.m_Gray = value;
            this.material = this.m_Gray ? MaterialUtil.GetDefaultSpriteGrayMaterial() : MaterialUtil.GetUIDefaultGraphicMaterial();
        }
    }




}



