using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public static class CameraUtility
{

    public static void AddCullingMask(this Camera _camera, int _layer)
    {
        if (_camera != null)
        {
            _camera.cullingMask |= (1 << _layer);
        }
    }

    public static void RemoveCullingMask(this Camera _camera, int _layer)
    {
        if (_camera != null)
        {
            _camera.cullingMask &= ~(1 << _layer);
        }
    }

}
