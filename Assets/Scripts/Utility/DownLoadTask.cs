using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;

public class DownLoadTask
{
    string url;
    string filePath;
    Action callBack;

    HttpWebRequest headFileRequest;
    WebResponse headFileResponse;

    bool stop = false;

    public DownLoadTask(string _url, string _filePath, Action _callBack)
    {
        this.url = _url;
        this.filePath = _filePath;
        this.callBack = _callBack;
    }

    public void Start()
    {
        RequestHeadFile();
    }

    public void Stop()
    {
        if (stop)
        {
            return;
        }

        stop = true;
    }

    private void RequestHeadFile()
    {
        headFileRequest = HttpWebRequest.CreateHttp(url);
        headFileRequest.BeginGetResponse(OnGotHeadFile, headFileRequest);
    }

    private void OnGotHeadFile(IAsyncResult _result)
    {
        headFileResponse = headFileRequest.EndGetResponse(_result);

    }

    private void CheckLocalFile()
    {
        var localFileSize = 0L;
        if (File.Exists(filePath))
        {
            localFileSize = new FileInfo(filePath).Length;
        }


    }

    private void RequestContent()
    {

    }

    private void OnGotContent(object _obj)
    {

    }


}
