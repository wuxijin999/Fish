using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public class Window : UIBase
{
    [Header("Base")]
    [SerializeField]
    int m_Id;
    public int id { get { return this.m_Id; } }
    [SerializeField] Tween m_Tween;
    [SerializeField] Button m_Close;
    [SerializeField] protected RectTransform m_BackGround;
    [SerializeField] protected RectTransform m_Content;

    List<Widget> widgets = new List<Widget>();
    Canvas m_Canvas;
    GraphicRaycaster m_Raycaster;

    public WindowState windowState { get; private set; }
    public int order = 1000;
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
    WindowConfig config { get { return WindowConfig.Get(this.m_Id); } }

    internal void Open(int _order)
    {
        this.order = _order;
        try
        {
            if (!this.initialized)
            {
                this.m_Canvas = this.GetComponent<Canvas>();
                this.m_Raycaster = this.GetComponent<GraphicRaycaster>();
                if (this.config.emptyToClose && this.emptyCloseButton == null)
                {
                    AddEmptyCloseResponser();
                }

                BindController();
                SetListeners();

                if (this.m_Close != null)
                {
                    this.m_Close.SetListener(() => { Close(); });
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
            if (this.m_Tween != null)
            {
                this.m_Tween.Play(true).OnComplete(this.OnAfterOpen);
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
                widget.rectTransform.MatchWhith(this.m_Content);
                widget.SetActive(true);
            }
        }
        else
        {
            DebugEx.LogFormat("{0}资源不存在，请检查！", this.name);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (this.m_BackGround == null)
        {
            this.m_BackGround = this.transform.GetComponent<RectTransform>("BackGround");
        }

        if (this.m_Content == null)
        {
            this.m_Content = this.transform.GetComponent<RectTransform>("Content");
        }
    }
#endif
}

public enum WindowState
{
    Closed,
    Opened,
}

