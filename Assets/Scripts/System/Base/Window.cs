using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(WindowInfo))]
public abstract class Window : MonoBehaviour
{
    [SerializeField] protected RectTransform m_BackGround;
    [SerializeField] protected RectTransform m_Content;

    int m_Function = 0;
    public int function
    {
        get { return m_Function; }
        private set { m_Function = value; }
    }

    WindowInfo m_WindowInfo = null;
    public WindowInfo windowInfo
    {
        get
        {
            return m_WindowInfo ?? (m_WindowInfo = this.GetComponent<WindowInfo>());
        }
    }

    public WindowState windowState
    {
        get; private set;
    }

    RectTransform rectTransform { get { return this.transform.AddMissingComponent<RectTransform>(); } }

    ButtonEx emptyCloseButton;
    bool initialized = false;
    float windowTimer = 0f;

    public bool playAnimation { get; set; }
    public int order = 1000;

    internal Window Open(int _function)
    {
        try
        {
            if (!initialized)
            {
                if (windowInfo.clickEmptyToClose && emptyCloseButton == null)
                {
                    AddEmptyCloseResponser();
                }

                BindController();
                AddListeners();
                initialized = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

        try
        {
            windowTimer = 0f;
            OnPreOpen();
            WindowCenter.Instance.NotifyBeforeOpen(this);
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

    internal Window Close(bool _immediately)
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
            windowInfo.interactable = false;
            WindowCenter.Instance.NotifyBeforeClose(this);
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

        try
        {
            WindowCenter.Instance.NotifyAfterClose(this);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }

        return this;
    }

    public virtual void CloseClick()
    {
        Close(true);
    }

    protected virtual void LateUpdate()
    {

    }

    protected virtual void OnActived()
    {

    }

    protected abstract void BindController();
    protected abstract void AddListeners();
    protected abstract void OnPreOpen();
    protected abstract void OnAfterOpen();
    protected abstract void OnPreClose();
    protected abstract void OnAfterClose();

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

        if (playAnimation && windowInfo.tween != null)
        {
            windowInfo.tween.Play(true);
        }
    }

    private void AddEmptyCloseResponser()
    {
        var emptyClose = UIUtility.CreateWidget("InvisibleButton", "EmptyClose");
        var rectTransform = emptyClose.transform as RectTransform;
        rectTransform.SetParentEx(this.transform, Vector3.zero, Quaternion.identity, Vector3.one);
        rectTransform.SetAsFirstSibling();
        rectTransform.MatchWhith(this.transform as RectTransform);
        emptyCloseButton = emptyClose.GetComponent<ButtonEx>();
        emptyCloseButton.AddListener(CloseClick);
    }

    public enum WindowState
    {
        Closed,
        Opened,
    }

}

