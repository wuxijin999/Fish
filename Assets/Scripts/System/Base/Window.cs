using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(WindowSetting))]
public class Window : UIBase
{
    WindowSetting m_Setting;
    protected WindowSetting setting { get { return m_Setting ?? (m_Setting = this.GetComponent<WindowSetting>()); } }
    WindowConfig config { get { return WindowConfig.Get(this.setting.id); } }

    List<Widget> widgets = new List<Widget>();
    Canvas m_Canvas;
    GraphicRaycaster m_Raycaster;

    public WindowState windowState { get; private set; }

    int m_Order = 1000;
    public int order {
        get { return m_Order; }
        set {
            m_Order = value;
            this.m_Canvas.sortingOrder = m_Order;
        }
    }

    bool m_Interactable = false;
    public bool interactable {
        get { return this.m_Interactable; }
        set {
            this.m_Interactable = value;
            this.m_Raycaster.enabled = this.m_Interactable;
        }
    }

    ButtonEx emptyCloseButton;
    bool initialized = false;

    internal void Open(int _order)
    {
        this.order = _order;
        try
        {
            if (!this.initialized)
            {
                this.m_Canvas = this.GetComponent<Canvas>();
                this.m_Raycaster = this.GetComponent<GraphicRaycaster>();
                this.m_Canvas.overrideSorting = true;
                this.m_Canvas.sortingLayerName = "UI";

                if (this.config.emptyToClose && this.emptyCloseButton == null)
                {
                    AddEmptyCloseResponser();
                }

                BindController();
                SetListeners();

                if (this.setting.close != null)
                {
                    this.setting.close.SetListener(Close);
                }
                this.initialized = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

        this.m_Canvas.sortingOrder = this.order;

        try
        {
            OnPreOpen();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        finally
        {
            this.rectTransform.MatchWhith(UIRoot.windowRoot);
            this.windowState = WindowState.Opened;
            ActiveWindow();
        }

    }

    internal void Close()
    {
        try
        {
            OnPreClose();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        try
        {
            this.interactable = false;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        finally
        {
            this.gameObject.SetActive(false);
            this.windowState = WindowState.Closed;
        }

        try
        {
            OnAfterClose();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

    }

    public void SetWidgetActive<T>(bool value) where T : Widget
    {
        var widget = this.widgets.Find((x) => { return x != null && x is T; });

        if (value)
        {
            if (widget != null)
            {
                widget.SetActive(true);
            }
            else
            {
                var name = typeof(T).Name;
                UIAssets.LoadWindowAsync(name, this.OnLoadWidget);
            }
        }
        else
        {
            if (widget != null)
            {
                widget.SetActive(false);
            }
        }
    }

    protected virtual void BindController() { }
    protected virtual void SetListeners() { }
    protected virtual void OnPreOpen() { }
    protected virtual void OnAfterOpen() { }
    protected virtual void OnPreClose() { }
    protected virtual void OnAfterClose() { }
    protected virtual void OnActived() { }

    private void ActiveWindow()
    {
        if (this.windowState == WindowState.Closed)
        {
            return;
        }

        this.enabled = true;

        if (!this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(true);
        }

        try
        {
            OnActived();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

        try
        {
            if (this.setting.tween != null)
            {
                this.setting.tween.Play(true).OnComplete(this.OnAfterOpen);
            }
            else
            {
                OnAfterOpen();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }

    }

    private void AddEmptyCloseResponser()
    {
        var emptyClose = UIUtil.CreateElement("InvisibleButton", "EmptyClose");
        var rectTransform = emptyClose.transform as RectTransform;
        rectTransform.MatchWhith(this.transform as RectTransform).SetAsFirstSibling();
        this.emptyCloseButton = emptyClose.GetComponent<ButtonEx>();
        this.emptyCloseButton.SetListener(() => { Close(); });
    }

    private void OnLoadWidget(bool ok, UnityEngine.Object @object)
    {
        if (ok && @object != null)
        {
            var prefab = @object as GameObject;
            var name = prefab.name;
            var exist = this.widgets.Exists((x) => { return x != null && x.name == name; });

            if (!exist)
            {
                var instance = GameObject.Instantiate(prefab);
                var widget = instance.GetComponent<Widget>();
                this.widgets.Add(widget);
                instance.name = name;
                UIAssets.UnLoadWindowAsset(name);
                widget.rectTransform.MatchWhith(this.setting.content);
                widget.SetActive(true);
            }
        }
        else
        {
            DebugEx.LogFormat("{0}资源不存在，请检查！", this.name);
        }
    }

}

public enum WindowState
{
    Closed,
    Opened,
}

