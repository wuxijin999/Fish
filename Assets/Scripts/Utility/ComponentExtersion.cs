using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public static class ComponentExtersion
{

    public static T AddMissingComponent<T>(this Component _compoent) where T : Component
    {
        if (_compoent == null)
        {
            return null;
        }

        T component = _compoent.GetComponent<T>();
        if (component == null)
        {
            component = _compoent.gameObject.AddComponent<T>();
        }

        return component;
    }

    public static T AddMissingComponent<T>(this Transform _transform) where T : Component
    {
        if (_transform == null)
        {
            return null;
        }

        T component = _transform.GetComponent<T>();
        if (component == null)
        {
            component = _transform.gameObject.AddComponent<T>();
        }

        return component;
    }

    public static T AddMissingComponent<T>(this GameObject _gameObject) where T : Component
    {
        if (_gameObject == null)
        {
            return null;
        }

        T component = _gameObject.GetComponent<T>();
        if (component == null)
        {
            component = _gameObject.AddComponent<T>();
        }

        return component;
    }

    public static void AddListener(this Button _button, UnityAction _action)
    {
        if (_button == null)
        {
            return;
        }
        _button.onClick.AddListener(_action);
    }

    public static void RemoveAllListeners(this Button _button)
    {
        if (_button == null)
        {
            return;
        }
        _button.onClick.RemoveAllListeners();
    }

    public static void AddListener(this Toggle _toggle, UnityAction<bool> _action)
    {
        if (_toggle == null)
        {
            return;
        }
        _toggle.onValueChanged.AddListener(_action);
    }

    public static void RemoveAllListeners(this Toggle _toggle)
    {
        if (_toggle == null)
        {
            return;
        }
        _toggle.onValueChanged.RemoveAllListeners();
    }

    public static void AddListener(this Slider _slider, UnityAction<float> _action)
    {
        if (_slider == null)
        {
            return;
        }
        _slider.onValueChanged.AddListener(_action);
    }

    public static void RemoveAllListeners(this Slider _slider)
    {
        if (_slider == null)
        {
            return;
        }
        _slider.onValueChanged.RemoveAllListeners();
    }

    public static void AddListener(this InputField _inputField, UnityAction<string> _action)
    {
        if (_inputField == null)
        {
            return;
        }
        _inputField.onValueChanged.AddListener(_action);
    }

    public static void RemoveAllListeners(this InputField _inputField)
    {
        if (_inputField == null)
        {
            return;
        }
        _inputField.onValueChanged.RemoveAllListeners();
    }


}
