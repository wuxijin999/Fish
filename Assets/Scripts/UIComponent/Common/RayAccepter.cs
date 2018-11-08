//--------------------------------------------------------------
//      [    Author   ]:                 Leonard.wu
//      [ CopyRight ]:                  
//--------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 空白的图形组件,这个组件可以影响UI事件,但是不会参与绘制
/// 即它不会影响DrawCall,也不会增加顶点
/// </summary>
[DisallowMultipleComponent]
public class RayAccepter:MaskableGraphic {

    protected override void OnPopulateMesh(VertexHelper vh) {
        base.OnPopulateMesh(vh);
        vh.Clear();
    }
}