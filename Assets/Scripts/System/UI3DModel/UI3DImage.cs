//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, November 05, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI3DImage : UIBase
{

    [SerializeField] RawImage m_RawImage;
    [SerializeField] GestureCatcher m_GestureCatcher;

    public void Display(DisplayParams @params)
    {

    }

    public void Dispose()
    {

    }

    public struct DisplayParams
    {
        public DisplayType type;
        public int id;
        public Vector3 offset;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public enum DisplayType
    {
        Hero,
        Player,
        Npc,
        Monster,
    }

}



