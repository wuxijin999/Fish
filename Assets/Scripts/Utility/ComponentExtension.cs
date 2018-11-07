using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public static class ComponentExtension
{

    public static void SetActive(this Component input, bool active)
    {
        if (input == null)
        {
            throw new ArgumentNullException();
        }

        if (active)
        {
            if (!input.gameObject.activeSelf)
            {
                input.gameObject.SetActive(true);
            }
        }
        else
        {
            if (input.gameObject.activeSelf)
            {
                input.gameObject.SetActive(false);
            }
        }
    }

    public static T AddMissingComponent<T>(this Component input) where T : Component
    {
        if (input == null)
        {
            return null;
        }

        T component = input.GetComponent<T>();
        if (component == null)
        {
            component = input.gameObject.AddComponent<T>();
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

    public static void SetListener(this Button button, UnityAction action)
    {
        if (button == null)
        {
            return;
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public static void AddListener(this Toggle toggle, UnityAction<bool> action)
    {
        if (toggle == null)
        {
            return;
        }
        toggle.onValueChanged.AddListener(action);
    }

    public static void SetListener(this Toggle toggle, UnityAction<bool> action)
    {
        if (toggle == null)
        {
            return;
        }
        toggle.onValueChanged.RemoveAllListeners();
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

    public static void SetListener(this Slider slider, UnityAction<float> action)
    {
        if (slider == null)
        {
            return;
        }
        slider.onValueChanged.RemoveAllListeners();
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

    public static void SetListener(this InputField inputField, UnityAction<string> action)
    {
        if (inputField == null)
        {
            return;
        }
        inputField.onValueChanged.RemoveAllListeners();
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

    public static void SetSprite(this Image image, int id)
    {
        if (image == null)
        {
            return;
        }

        var config = IconConfig.Get(id);
        if (config != null)
        {
            image.overrideSprite = UIAssets.LoadSprite(config.folder, config.assetName);
        }
    }

    public static void SetText(this TextEx text, object @object)
    {
        if (text != null)
        {
            text.SetText(@object.ToString());
        }
    }

    public static void SetColor(this TextEx text, Color color)
    {
        if (text != null)
        {
            text.color = color;
        }
    }

    public static void SetLanguage(this TextEx text, int id)
    {
        if (text != null)
        {
            text.SetText(Language.Get(id));
        }
    }

}
