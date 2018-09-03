using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public static class CameraUtil
{

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

}
