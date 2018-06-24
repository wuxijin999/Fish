using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CameraUtility
{
    public static void AddCullingMask(Camera _camera, int _layer)
    {
        _camera.cullingMask |= (1 << _layer);
    }

    public static void RemoveCullingMask(Camera _camera, int _layer)
    {
        _camera.cullingMask &= ~(1 << _layer);
    }

    static bool screenShotCutTextureCreated = false;
    static Rect screenRect;
    static Stack<Texture2D> screenShotCutTexturePool = new Stack<Texture2D>();

    static Texture2D RequireTexture2D()
    {
        if (screenShotCutTexturePool.Count > 0)
        {
            return screenShotCutTexturePool.Pop();
        }
        else
        {
            return new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }
    }

    static void RecycleTexture2D(Texture2D _texture)
    {
        if (_texture != null)
        {
            screenShotCutTexturePool.Push(_texture);
        }
    }

    public static void StopShotCut(RawImage _image)
    {
        if (_image.texture != null)
        {
            RecycleTexture2D((Texture2D)_image.texture);
        }
    }

}
