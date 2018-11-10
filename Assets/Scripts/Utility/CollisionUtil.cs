using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionUtil
{

    public static bool TryGetGroundHeight(Vector3 postion, out float height)
    {
        RaycastHit hit;
        var ray = new Ray(postion.SetY(150f), Vector3.down);

        if (Physics.Raycast(ray, out hit, 200f, LayerUtil.WalkbleMask))
        {
            height = hit.point.y;
            return true;
        }
        else
        {
            height = 0f;
            return false;
        }
    }

}
