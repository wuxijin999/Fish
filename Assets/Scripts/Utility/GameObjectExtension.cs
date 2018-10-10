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

    public static T CreateMonoBehaviour<T>(string name) where T : MonoBehaviour
    {
        var gameObject = new GameObject(name);
        return gameObject.AddComponent<T>();
    }


}
