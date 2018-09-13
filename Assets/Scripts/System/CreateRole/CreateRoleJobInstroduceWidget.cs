//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, September 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class CreateRoleJobInstroduceWidget : MonoBehaviour {

    RectTransform m_RecTransform;
    RectTransform rectTransform { get { return m_RecTransform??(this.transform as RectTransform); } }

}



