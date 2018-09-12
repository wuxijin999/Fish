﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class Window : MonoBehaviour
{

    [SerializeField] int m_Id;
    public int id { get { return m_Id; } }

    [SerializeField] Tween m_Tween;
    [SerializeField] Button m_Close;
    [SerializeField] protected RectTransform m_BackGround;
    [SerializeField] protected RectTransform m_Content;

    Canvas m_Canvas;
    GraphicRaycaster m_Raycaster;

    public WindowState windowState { get; private set; }
    public int order = 1000;
    bool m_Interactable = false;
    public bool interactable
    {
        get { return m_Interactable; }
        set
        {
            m_Interactable = value;
            m_Raycaster.enabled = m_Interactable;
        }
    }

    ButtonEx emptyCloseButton;
    bool initialized = false;
    WindowConfig config { get { return WindowConfig.Get(m_Id); } }
    RectTransform rectTransform { get { return this.transform.AddMissingComponent<RectTransform>(); } }

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
            rectTransform.MatchWhith(UIRoot.Instance.windowRoot);
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

    protected virtual void BindController() { }
    protected virtual void SetListeners() { }
    protected virtual void OnPreOpen() { }
    protected virtual void OnAfterOpen() { }
    protected virtual void OnPreClose() { }
    protected virtual void OnAfterClose() { }
    protected virtual void OnActived() { }
    protected virtual void LateUpdate() { }

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
        var emptyClose = UIUtil.CreateWidget("InvisibleButton", "EmptyClose");
        var rectTransform = emptyClose.transform as RectTransform;
        rectTransform.MatchWhith(this.transform as RectTransform).SetAsFirstSibling();
        emptyCloseButton = emptyClose.GetComponent<ButtonEx>();
        emptyCloseButton.SetListener(() => { Close(); });
    }

}

public enum WindowState
{
    Closed,
    Opened,
}

