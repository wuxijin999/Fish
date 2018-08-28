using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowCenter : Singleton<WindowCenter>
{
    public event Action<Window> windowBeforeOpenEvent;
    public event Action<Window> windowAfterOpenEvent;
    public event Action<Window> windowBeforeCloseEvent;
    public event Action<Window> windowAfterCloseEvent;

    List<string> closeAllIgnoreWindows = new List<string>() { };

    UIRoot m_UIRoot;
    public UIRoot uiRoot
    {
        get
        {
            if (m_UIRoot == null)
            {
                var prefab = Resources.Load<GameObject>("UI/Prefabs/UIRoot");
                var instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                instance.name = "UIRoot";
                m_UIRoot = instance.GetComponent<UIRoot>();
                if (Application.isPlaying)
                {
                    GameObject.DontDestroyOnLoad(instance);
                }
            }
            return m_UIRoot;
        }
    }

    WindowAsyncLoad m_AnyncLoad;
    public WindowAsyncLoad asyncLoad
    {
        get
        {
            if (m_AnyncLoad == null)
            {
                var gameObject = new GameObject("WindowAnyncLoad");
                m_AnyncLoad = gameObject.AddMissingComponent<WindowAsyncLoad>();
                GameObject.DontDestroyOnLoad(gameObject);
            }

            return m_AnyncLoad;
        }
    }

    Dictionary<string, Window> windows = new Dictionary<string, Window>();

    public void OpenFromLocal<T>() where T : Window
    {
        var windowName = typeof(T).Name;
        T win = null;
        if (TryGetWindow(out win))
        {
            if (win.windowState == Window.WindowState.Closed)
            {
                win.Open(0);
            }
            else
            {
                DebugEx.Log(string.Format("{0} 窗口已经打开！", typeof(T)));
            }
        }
        else
        {
            win = GetWindowInstance<T>(true);
            if (win != null)
            {
                win.Open(0);
            }
        }
    }

    public T Open<T>(bool _forceSync = false, int _functionalOrder = 0) where T : Window
    {
        return OpenSingleWindow<T>(_forceSync, _functionalOrder);
    }

    public T Get<T>() where T : Window
    {
        T win = null;
        if (TryGetWindow(out win))
        {
            return win;
        }
        else
        {
            DebugEx.LogFormat("没有找到窗口:{0}", typeof(T).Name);
            return null;
        }
    }

    public Window Get(string _window)
    {
        if (windows.ContainsKey(_window))
        {
            return windows[_window];
        }
        else
        {
            return null;
        }
    }

    public T Close<T>(bool _immediately) where T : Window
    {
        T win = null;
        if (TryGetWindow<T>(out win))
        {
            if (win.windowState != Window.WindowState.Closed)
            {
                win.Close(_immediately);
            }
            else
            {
                DebugEx.Log(string.Format("{0} 窗口已经关闭！", typeof(T)));
            }
        }
        else
        {
            asyncLoad.StopTask(typeof(T).Name);
            DebugEx.Log(string.Format("{0} 窗口无法获得！", typeof(T)));
        }

        return win;
    }

    public List<string> GetAll()
    {
        return new List<string>(windows.Keys);
    }

    public void CloseOthers<T>() where T : Window
    {
        foreach (var window in windows.Values)
        {
            if (window is T)
            {
                continue;
            }

            if (window != null)
            {
                if (window.windowState == Window.WindowState.Opened)
                {
                    window.Close(true);
                }
            }
        }

        asyncLoad.StopAllTasks();
    }

    public void DestroyWin<T>() where T : Window
    {
        T win = null;
        if (TryGetWindow<T>(out win))
        {
            win.Close(true);
            GameObject.Destroy(win.gameObject);
            MonoBehaviour.Destroy(win);

            var name = typeof(T).Name;
            windows[name] = null;
            windows.Remove(typeof(T).Name);
        }
        else
        {
            DebugEx.Log(string.Format("{0} 窗口无法获得！", typeof(T)));
        }
    }

    public void UnLoadAssetBundle(WindowStage _windowStage)
    {
        if (!AssetSource.uiFromEditor)
        {
            switch (_windowStage)
            {
                case WindowStage.Launch:
                    break;
                case WindowStage.Login:
                    break;
                case WindowStage.SelectRole:
                    break;
                default:
                    break;
            }
        }
    }

    public bool CheckOpen<T>() where T : Window
    {
        T win = null;
        if (TryGetWindow(out win))
        {
            var open = win.windowState == Window.WindowState.Opened;
            return open;
        }
        else
        {
            return false;
        }
    }

    public bool CheckOpen(string _windowName)
    {
        if (windows.ContainsKey(_windowName) && windows[_windowName] != null)
        {
            var window = windows[_windowName];
            return window.windowState == Window.WindowState.Opened;
        }
        else
        {
            return false;
        }
    }

    public bool ExitAnyFullScreenWin()
    {
        bool exit = false;
        foreach (var window in windows.Values)
        {
            if (window.windowInfo.fullScreen)
            {
                if (window.windowState == Window.WindowState.Opened)
                {
                    exit = true;
                    break;
                }
            }
        }

        return exit;
    }

    internal void NotifyBeforeOpen<T>(T _window) where T : Window
    {
        if (windowBeforeOpenEvent != null)
        {
            windowBeforeOpenEvent(_window);
        }
    }

    internal void NotifyAfterOpen<T>(T _window) where T : Window
    {
        if (windowAfterOpenEvent != null)
        {
            windowAfterOpenEvent(_window);
        }
    }

    internal void NotifyBeforeClose<T>(T _window) where T : Window
    {
        if (windowBeforeCloseEvent != null)
        {
            windowBeforeCloseEvent(_window);
        }
    }

    internal void NotifyAfterClose<T>(T _window) where T : Window
    {
        if (windowAfterCloseEvent != null)
        {
            windowAfterCloseEvent(_window);
        }
    }

    private T OpenSingleWindow<T>(bool _forceSync, int _functionalOrder) where T : Window
    {
        T win = null;
        if (TryGetWindow(out win))
        {
            if (win.windowState == Window.WindowState.Closed)
            {
                win.Open(_functionalOrder);
            }
            else
            {
                DebugEx.Log(string.Format("{0} 窗口已经打开！", typeof(T)));
            }

            return (T)win;
        }

        if (_forceSync || AssetSource.uiFromEditor)
        {
            win = GetWindowInstance<T>(false);
            if (win != null)
            {
                win.Open(_functionalOrder);
            }

            return (T)win;
        }
        else
        {
            GetWindowInstanceAsync<T>(
                (bool ok, UnityEngine.Object _object) =>
                {
                    if (TryGetWindow(out win))
                    {
                        if (win.windowState == Window.WindowState.Closed)
                        {
                            win.Open(_functionalOrder);
                        }
                        else
                        {
                            DebugEx.Log(string.Format("{0} 窗口已经打开！", typeof(T)));
                        }
                    }
                }
                );

            return null;
        }
    }

    private bool TryGetWindow<T>(out T _win) where T : Window
    {
        var windowName = typeof(T).Name;
        WindowTrim(windowName);

        if (windows.ContainsKey(windowName))
        {
            _win = (T)windows[windowName];
            return true;
        }
        else
        {
            _win = null;
            return false;
        }
    }

    private void WindowTrim(string _windowName)
    {
        if (windows.ContainsKey(_windowName))
        {
            if (windows[_windowName] == null || windows[_windowName].gameObject == null)
            {
                windows.Remove(_windowName);
            }
        }
    }

    private T GetWindowInstance<T>(bool _fromLocal) where T : Window
    {
        var prefabName = typeof(T).Name;

        if (windows.ContainsKey(prefabName))
        {
            return windows[prefabName] as T;
        }
        else
        {
            var prefab = _fromLocal ? Resources.Load<GameObject>(StringUtility.Contact("UI/Prefabs/", prefabName))
                : UIAssets.LoadWindow(prefabName);

            prefab.SetActive(false);
            var instance = GameObject.Instantiate(prefab);
            if (AssetSource.uiFromEditor)
            {
                prefab.SetActive(true);
            }

            UIAssets.UnLoadWindowAsset(prefabName);
            instance.name = prefabName;
            var window = instance.GetComponent<T>();
            if (window != null)
            {
                var windowName = typeof(T).Name;
                windows[windowName] = window;
            }
            else
            {
                DebugEx.LogFormat("无法获得  {0}  的资源！", prefabName);
            }

            return window;
        }

    }

    private void GetWindowInstanceAsync<T>(Action<bool, UnityEngine.Object> _callBack) where T : Window
    {
        GetWindowInstanceAsync(typeof(T).Name, _callBack);
    }

    private void GetWindowInstanceAsync(string _windowName, Action<bool, UnityEngine.Object> _callBack)
    {
        Action<bool, UnityEngine.Object> addAction = (bool _ok, UnityEngine.Object _object) =>
        {
            var prefabName = _windowName;
            Window window = null;
            if (!windows.ContainsKey(_windowName))
            {
                var prefab = _object as GameObject;
                prefab.SetActive(false);
                var instance = GameObject.Instantiate(prefab);

                if (AssetSource.uiFromEditor)
                {
                    prefab.SetActive(true);
                }

                UIAssets.UnLoadWindowAsset(prefabName);
                instance.name = _windowName;
                window = (Window)instance.GetComponent(_windowName);
                if (window != null)
                {
                    windows[_windowName] = (Window)window;
                }
                else
                {
                    Debug.LogFormat("无法获得  {0}  的资源！", _windowName);
                }
            }

            if (_callBack != null)
            {
                _callBack(_ok && window != null, _object);
            }
        };

        asyncLoad.PushTask(new WindowAsyncLoad.Task(_windowName, addAction));
    }

    public enum WindowStage
    {
        Launch,
        Login,
        SelectRole,
        Other,
    }

}

