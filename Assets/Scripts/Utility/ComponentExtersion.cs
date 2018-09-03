using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public static class ComponentExtersion
{

    public static T AddMissingComponent<T>(this Component compoent) where T : Component
    {
        if (compoent == null)
        {
            return null;
        }

        T component = compoent.GetComponent<T>();
        if (component == null)
        {
            component = compoent.gameObject.AddComponent<T>();
        }

        return component;
    }

    public static T AddMissingComponent<T>(this Transform transform) where T : Component
    {
        if (transform == null)
        {
            return null;
        }

        T component = transform.GetComponent<T>();
        if (component == null)
        {
            component = transform.gameObject.AddComponent<T>();
        }

        return component;
    }

    public static T AddMissingComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    public static void AddListener(this Button button, UnityAction action)
    {
        if (button == null)
        {
            return;
        }
        button.onClick.AddListener(action);
    }

    public static void RemoveAllListeners(this Button button)
    {
        if (button == null)
        {
            return;
        }
        button.onClick.RemoveAllListeners();
    }

    public static void AddListener(this Toggle toggle, UnityAction<bool> action)
    {
        if (toggle == null)
        {
            return;
        }
        toggle.onValueChanged.AddListener(action);
    }

    public static void RemoveAllListeners(this Toggle toggle)
    {
        if (toggle == null)
        {
            return;
        }
        toggle.onValueChanged.RemoveAllListeners();
    }

    public static void AddListener(this Slider slider, UnityAction<float> action)
    {
        if (slider == null)
        {
            return;
        }
        slider.onValueChanged.AddListener(action);
    }

    public static void RemoveAllListeners(this Slider slider)
    {
        if (slider == null)
        {
            return;
        }
        slider.onValueChanged.RemoveAllListeners();
    }

    public static void AddListener(this InputField inputField, UnityAction<string> action)
    {
        if (inputField == null)
        {
            return;
        }
        inputField.onValueChanged.AddListener(action);
    }

    public static void RemoveAllListeners(this InputField inputField)
    {
        if (inputField == null)
        {
            return;
        }
        inputField.onValueChanged.RemoveAllListeners();
    }


}
