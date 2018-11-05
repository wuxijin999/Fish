//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, October 13, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;

public class UI3DModelDrawer : UIBase
{

    [SerializeField] Vector2 m_RenderTextureSize;

    RenderTexture renderTexture;

    public virtual void Display(int objectId, Vector3 offset, Vector3 rotation, Vector3 scale)
    {
        if (renderTexture == null)
        {
            CreateRenderTexture();
        }

    }

    private void CreateRenderTexture()
    {
        var width = Mathf.RoundToInt(m_RenderTextureSize.x);
        var heigth = Mathf.RoundToInt(m_RenderTextureSize.y);
        renderTexture = new RenderTexture(width, heigth, 16);

        renderTexture.format = RenderTextureFormat.ARGB4444;
        renderTexture.useMipMap = false;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.filterMode = FilterMode.Bilinear;
    }

}



