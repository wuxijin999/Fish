using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{

    public static void DestroySelf(this GameObject gameObject)
    {
        if (gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }
    }




}
