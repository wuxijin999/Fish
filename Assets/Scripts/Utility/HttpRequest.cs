using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

public class HttpRequest : Singleton<HttpRequest>
{
    public const string defaultHttpContentType = "application/x-www-form-urlencoded";

    public void RequestHttpPost(string url, string content, Action<bool, string> result = null)
    {
        HttpAsyncHandle.Create(url, WebRequestMethods.Http.Post, content, result);
    }

    public void RequestHttpGet(string url, Action<bool, string> result = null)
    {
        HttpAsyncHandle.Create(url, WebRequestMethods.Http.Get, "", result);
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