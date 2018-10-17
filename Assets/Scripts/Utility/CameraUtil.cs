using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public static class CameraUtil
{

    public static Camera fightCamera;

    public static void AddCullingMask(this Camera camera, int layer)
    {
        if (camera != null)
        {
            camera.cullingMask |= (1 << layer);
        }
    }

    public static void RemoveCullingMask(this Camera camera, int layer)
    {
        if (camera != null)
        {
            camera.cullingMask &= ~(1 << layer);
        }
    }

    public static Vector3 ConvertPosition(Camera from, Camera to, Vector3 position)
    {
        if (from == null || to == null)
        {
            return Vector3.zero;
        }

        var vpPoint = from.WorldToViewportPoint(position);
        var toPosition = to.ViewportToWorldPoint(vpPoint);
        return toPosition;
    }
   
}
