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

    static GameObjectPool m_Pool;
    static GameObjectPool pool
    {
        get
        {
            if (m_Pool == null)
            {
                var carrier = new GameObject();
                var behaviour = carrier.AddComponent<HttpAsyncHandle>();
                m_Pool = GameObjectPoolUtil.Create(carrier);
            }

            return m_Pool;
        }
    }

    public static void Create(string url, string method, string content, Action<bool, string> result = null)
    {
        var behaviour = pool.Get().GetComponent<HttpAsyncHandle>();

        behaviour.gameObject.SetActive(true);
        behaviour.Reinitialize();
        behaviour.Begin(url, method, content, result);
    }

    public void Reinitialize()
    {
        this.url = string.Empty;
        this.method = string.Empty;
        this.content = string.Empty;
        this.message = string.Empty;

        this.callBack = null;
        this.getResult = false;
        this.timeOut = 0f;
        this.cookie = null;
        this.request = null;
        this.ok = false;
    }

    public void Begin(string url, string method, string content, Action<bool, string> result = null)
    {
        this.url = url;
        this.method = method;
        this.content = content;
        this.callBack = result;
        this.timeOut = Time.time + DESIGN_TIME_OUT_SECOND;

        this.cookie = new CookieContainer();
        this.request = (HttpWebRequest)WebRequest.Create(url);
        this.request.ServicePoint.Expect100Continue = false;
        this.request.Method = method;
        this.request.ContentType = HttpRequest.defaultHttpContentType;
        this.request.CookieContainer = this.cookie;
        this.request.Proxy = null;
        this.request.KeepAlive = false;

        try
        {
            if (string.IsNullOrEmpty(content))
            {
                this.request.BeginGetResponse(this.OnHttpWebResponse, null);
            }
            else
            {
                var data = Encoding.UTF8.GetBytes(content);
                this.request.ContentLength = data.Length;
                this.request.BeginGetRequestStream(this.GetRequestStreamCallback, null);
            }
        }
        catch (System.Exception ex)
        {
            this.ok = false;
            this.message = ex.Message;
            this.getResult = true;
        }
    }

    void Update()
    {
        if (Time.time > this.timeOut && !this.getResult)
        {
            this.request.Abort();
            this.ok = false;
            this.message = "TimeOut";
            this.getResult = true;
        }

        if (this.getResult)
        {
            if (this.request != null)
            {
                this.request.Abort();
            }

            if (this.callBack != null)
            {
                this.callBack(this.ok, this.message);
                this.callBack = null;
                DebugEx.LogFormat("Http 数据通信 {0},请求数据结果：{1},内容：{2}", this.method, this.ok, this.message);
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
            var bytes = Encoding.UTF8.GetBytes(this.content);
            s = this.request.EndGetRequestStream(ar);
            s.Write(bytes, 0, bytes.Length);
            this.request.BeginGetResponse(this.OnHttpWebResponse, this.request);
        }
        catch (Exception ex)
        {
            this.ok = false;
            this.message = ex.Message;
            this.getResult = true;
        }
        finally
        {
            if (s != null)
            {
                s.Close();
            }
        }
    }

    private void OnHttpWebResponse(IAsyncResult result)
    {
        HttpWebResponse response = null;
        Stream s = null;
        StreamReader sr = null;

        try
        {
            response = this.request.EndGetResponse(result) as HttpWebResponse;
            response.Cookies = this.cookie.GetCookies(response.ResponseUri);
            s = response.GetResponseStream();
            sr = new StreamReader(s, Encoding.UTF8);
            this.message = sr.ReadToEnd();
            this.ok = true;
        }
        catch (System.Exception ex)
        {
            this.ok = false;
            this.message = ex.Message;
        }
        finally
        {
            if (this.request != null)
            {
                this.request.Abort();
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
            this.getResult = true;
        }

    }

}
