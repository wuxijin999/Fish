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

    [SerializeField] int m_Id;
    public int id { get { return m_Id; } }

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
        get { return m_Interactable; }
        set {
            m_Interactable = value;
            m_Raycaster.enabled = m_Interactable;
        }
    }

    ButtonEx emptyCloseButton;
    bool initialized = false;
    WindowConfig config { get { return WindowConfig.Get(m_Id); } }

    internal Window Open(int _order)
    {
        order = _order;
        try
        {
            if (!initialized)
            {
                m_Canvas = this.GetComponent<Canvas>();
                m_Raycaster = this.GetComponent<GraphicRaycaster>();
                if (config.emptyToClose && emptyCloseButton == null)
                {
                    AddEmptyCloseResponser();
                }

                BindController();
                SetListeners();

                if (m_Close != null)
                {
                    m_Close.SetListener(() => { Close(); });
                }
                initialized = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

        m_Canvas.sortingOrder = order;

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
            rectTransform.MatchWhith(UIRoot.windowRoot);
            windowState = WindowState.Opened;
            ActiveWindow();
        }

        return this;
    }

    internal Window Close()
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
            interactable = false;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        finally
        {
            this.gameObject.SetActive(false);
            windowState = WindowState.Closed;
        }

        try
        {
            OnAfterClose();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

        return this;
    }

    public void SetWidgetActive<T>(bool value) where T : Widget
    {
        var widget = widgets.Find((x) => { return x != null && x is T; });

        if (value)
        {
            if (widget != null)
            {
                widget.SetActive(true);
            }
            else
            {
                var name = typeof(T).Name;
                UIAssets.LoadWindowAsync(name, OnLoadWidget);
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
        if (windowState == WindowState.Closed)
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
            if (m_Tween != null)
            {
                m_Tween.Play(true).OnComplete(OnAfterOpen);
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
        emptyCloseButton = emptyClose.GetComponent<ButtonEx>();
        emptyCloseButton.SetListener(() => { Close(); });
    }

    private void OnLoadWidget(bool ok, UnityEngine.Object @object)
    {
        if (ok && @object != null)
        {
            var prefab = @object as GameObject;
            var name = prefab.name;
            var exist = widgets.Exists((x) => { return x != null && x.name == name; });

            if (!exist)
            {
                var instance = GameObject.Instantiate(prefab);
                var widget = instance.GetComponent<Widget>();
                widgets.Add(widget);
                instance.name = name;
                UIAssets.UnLoadWindowAsset(name);
                widget.rectTransform.MatchWhith(m_Content);
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

