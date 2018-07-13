using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class HttpRequest : SingletonMonobehaviour<HttpRequest>
{
    public const string defaultHttpContentType = "application/x-www-form-urlencoded";
    public const string jsonHttpContentType = "application/json ; charset=utf-8";

    public void RequestWWW(string _url, int _retry = 3, Action<bool, string> _result = null)
    {
        StartCoroutine(Co_RequestWWW(_url, null, _retry, _result));
    }

    public void RequestWWW(string _url, IDictionary<string, string> _parameters, int _retry = 3, Action<bool, string> _result = null)
    {
        StartCoroutine(Co_RequestWWW(_url, _parameters, _retry, _result));
    }

    IEnumerator Co_RequestWWW(string _url, IDictionary<string, string> _parameters, int _retry = 3, Action<bool, string> _result = null)
    {
        if (_url == null || _url.Length == 0)
        {
            DebugEx.LogError("PHPDataComm post 参数有错");
            if (_result != null)
            {
                _result(false, string.Empty);
                _result = null;
            }
            yield break;
        }

        int i = 0;
        bool isSuccess = false;

        byte[] data = null;
        if (_parameters != null)
        {
            data = Encoding.UTF8.GetBytes(HashtablaToString(_parameters));
        }

        var PostData = data == null ? new WWW(_url) : new WWW(_url, data);

        while (!PostData.isDone)
        {
            yield return null;
        }

        if (PostData.error != null)
        {
            Debug.LogErrorFormat("WWW 数据通信,请求数据失败：{0},已经尝试,{1},次", PostData.error, i);
        }
        else
        {
            if (!string.IsNullOrEmpty(PostData.text))
            {
                DebugEx.LogFormat("WWW 数据通信,请求数据成功：{0}", PostData.text);
                isSuccess = true;
                if (_result != null)
                {
                    _result(true, PostData.text);
                    _result = null;
                }
            }
        }

        if (!isSuccess)
        {
            if (_result != null)
            {
                _result(false, string.Empty);
                _result = null;
            }
        }
    }

    public void RequestHttpPost(string _url, IDictionary<string, string> _parameters, string _contentType, int _retry = 3, Action<bool, string> _result = null)
    {
        var content = HashtablaToString(_parameters);
        StartCoroutine(Co_RequestHttp(_url, "POST", content, _contentType, _retry, _result));
    }

    public void RequestHttpPost(string _url, string _content, string _contentType, int _retry = 3, Action<bool, string> _result = null)
    {
        StartCoroutine(Co_RequestHttp(_url, "POST", _content, _contentType, _retry, _result));
    }

    public void RequestHttpGet(string _url, string _contentType, int _retry = 3, Action<bool, string> _result = null)
    {
        StartCoroutine(Co_RequestHttp(_url, "GET", "", _contentType, _retry, _result));
    }

    IEnumerator Co_RequestHttp(string _url, string _method, string _content, string _contentType, int _retry = 3, Action<bool, string> _result = null)
    {
        bool bSucceed = false;
        int retry = 0;
        string retMessage = "";
        while (retry < _retry && !bSucceed)
        {
            retry++;
            var cookie = new CookieContainer();
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.ServicePoint.Expect100Continue = false;
            request.Method = _method;
            request.ContentType = _contentType;
            request.CookieContainer = cookie;
            request.Timeout = 2000;
            request.ReadWriteTimeout = 2000;

            if (_method == "POST")
            {
                byte[] data = Encoding.UTF8.GetBytes(_content);
                request.ContentLength = data.Length;
                try
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch (System.Exception ex)
                {
                    DebugEx.LogError(ex);
                    request.Abort();
                    bSucceed = false;
                    retMessage = ex.Message;
                    continue;
                }
            }

            IAsyncResult asyncResult = null;
            try
            {
                asyncResult = request.BeginGetResponse(null, null);
            }
            catch (System.Exception ex)
            {
                DebugEx.LogError(ex);
                request.Abort();
                bSucceed = false;
                retMessage = ex.Message;
                continue;
            }

            if (asyncResult == null)
            {
                request.Abort();
                bSucceed = false;
                retMessage = "asyncResult is null!";
                continue;
            }

            while (!asyncResult.IsCompleted)
            {
                yield return null;
            }

            HttpWebResponse response = null;
            try
            {
                response = request.EndGetResponse(asyncResult) as HttpWebResponse;
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();
                request.Abort();
                bSucceed = true;
                retMessage = retString;
                break;
            }
            catch (System.Exception ex)
            {
                bSucceed = false;
                if (response != null)
                {
                    response.Close();
                }
                request.Abort();
                DebugEx.LogError(ex);
                retMessage = ex.Message;
            }

            Debug.LogErrorFormat("Http 数据通信:{0}请求数据失败：{1} 已经尝试  {2} 次", _method, retMessage, retry);
            yield return WaitingForSecondConst.WaitMS100;
        }

        if (_result != null)
        {
            _result(bSucceed, retMessage);
            _result = null;
            DebugEx.LogFormat("Http 数据通信 {0},请求数据结果：{1},内容：{2}", _method, bSucceed, retMessage);
        }
    }

    static StringBuilder buffer = new StringBuilder();
    public static string HashtablaToString(IDictionary<string, string> parameters)
    {
        buffer.Remove(0, buffer.Length);
        int i = 0;
        foreach (KeyValuePair<string, string> item in parameters)
        {
            if (i > 0)
            {
                buffer.AppendFormat("&{0}={1}", item.Key, item.Value);
            }
            else
            {
                buffer.AppendFormat("{0}={1}", item.Key, item.Value);
            }
            i++;
        }
        string result = buffer.ToString();
        return result;
    }

}