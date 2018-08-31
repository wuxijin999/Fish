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
            return m_Gray;
        }
        set
        {
            m_Gray = value;
            this.material = m_Gray ? MaterialUtil.GetDefaultSpriteGrayMaterial() : MaterialUtil.GetUIDefaultGraphicMaterial();
        }
    }




}



