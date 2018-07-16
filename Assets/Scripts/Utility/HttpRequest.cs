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

    public void RequestHttpPost(string _url, string _content,  Action<bool, string> _result = null)
    {
        HttpAsyncHandle.Create(_url, "POST", _content,  _result);
    }

    public void RequestHttpGet(string _url,  Action<bool, string> _result = null)
    {
        HttpAsyncHandle.Create(_url, "GET", "",  _result);
    }

    static StringBuilder buffer = new StringBuilder();
    public static string HashTableToString(IDictionary<string, string> parameters)
    {
        buffer.Remove(0, buffer.Length);
        var index = 0;
        foreach (var item in parameters)
        {
            buffer.AppendFormat(index > 0 ? "{0}={1}" : "&{0}={1}", item.Key, item.Value);
            index++;
        }

        return buffer.ToString();
    }

}