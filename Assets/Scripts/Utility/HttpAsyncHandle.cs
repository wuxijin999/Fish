using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class HttpAsyncHandle : MonoBehaviour
{
    const int DESIGN_TIME_OUT_SECOND = 5;

    string url;
    string method;
    string content;

    Action<bool, string> callBack;

    bool getResult = false;
    float timeOut = 0f;
    CookieContainer cookie;
    HttpWebRequest request;
    bool ok = false;
    string message = string.Empty;

    static UIGameObjectPool m_Pool;
    static UIGameObjectPool pool {
        get {
            if (m_Pool == null)
            {
                var carrier = new GameObject();
                var behaviour = carrier.AddComponent<HttpAsyncHandle>();
                m_Pool = UIGameObjectPoolUtility.Create(carrier);
            }

            return m_Pool;
        }
    }

    public static void Create(string _url, string _method, string _content, Action<bool, string> _result = null)
    {
        var behaviour = pool.Get().GetComponent<HttpAsyncHandle>();

        behaviour.gameObject.SetActive(true);
        behaviour.Reinitialize();
        behaviour.Begin(_url, _method, _content, _result);
    }

    public void Reinitialize()
    {
        url = string.Empty;
        method = string.Empty;
        content = string.Empty;
        message = string.Empty;

        callBack = null;
        getResult = false;
        timeOut = 0f;
        cookie = null;
        request = null;
        ok = false;
    }

    public void Begin(string _url, string _method, string _content, Action<bool, string> _result = null)
    {
        this.url = _url;
        this.method = _method;
        this.content = _content;
        this.callBack = _result;
        this.timeOut = Time.time + DESIGN_TIME_OUT_SECOND;

        cookie = new CookieContainer();
        request = (HttpWebRequest)WebRequest.Create(_url);
        request.ServicePoint.Expect100Continue = false;
        request.Method = _method;
        request.ContentType = HttpRequest.defaultHttpContentType;
        request.CookieContainer = cookie;
        request.Proxy = null;
        request.KeepAlive = false;

        try
        {
            if (string.IsNullOrEmpty(_content))
            {
                request.BeginGetResponse(OnHttpWebResponse, null);
            }
            else
            {
                var data = Encoding.UTF8.GetBytes(_content);
                request.ContentLength = data.Length;
                request.BeginGetRequestStream(GetRequestStreamCallback, null);
            }
        }
        catch (System.Exception ex)
        {
            ok = false;
            message = ex.Message;
            getResult = true;
        }
    }

    void Update()
    {
        if (Time.time > timeOut && !getResult)
        {
            request.Abort();
            ok = false;
            message = "TimeOut";
            getResult = true;
        }

        if (getResult)
        {
            if (request != null)
            {
                request.Abort();
            }

            if (callBack != null)
            {
                callBack(ok, message);
                callBack = null;
                DebugEx.LogFormat("Http 数据通信 {0},请求数据结果：{1},内容：{2}", method, ok, message);
            }

            this.gameObject.SetActive(false);
            pool.Release(this.gameObject);
        }
    }

    private void GetRequestStreamCallback(IAsyncResult ar)
    {
        Stream s = null;
        try
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            s = request.EndGetRequestStream(ar);
            s.Write(bytes, 0, bytes.Length);
            request.BeginGetResponse(OnHttpWebResponse, request);
        }
        catch (Exception ex)
        {
            ok = false;
            message = ex.Message;
            getResult = true;
        }
        finally
        {
            if (s != null)
            {
                s.Close();
            }
        }
    }

    private void OnHttpWebResponse(IAsyncResult _result)
    {
        HttpWebResponse response = null;
        Stream s = null;
        StreamReader sr = null;

        try
        {
            response = request.EndGetResponse(_result) as HttpWebResponse;
            response.Cookies = cookie.GetCookies(response.ResponseUri);
            s = response.GetResponseStream();
            sr = new StreamReader(s, Encoding.UTF8);
            message = sr.ReadToEnd();
            ok = true;
        }
        catch (System.Exception ex)
        {
            ok = false;
            message = ex.Message;
        }
        finally
        {
            if (request != null)
            {
                request.Abort();
            }

            if (response != null)
            {
                response.Close();
            }

            if (s != null)
            {
                s.Close();
            }

            if (sr != null)
            {
                sr.Close();
            }
            getResult = true;
        }

    }

}
