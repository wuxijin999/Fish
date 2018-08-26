using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;

public class DownLoadTask
{

    public const int MaxConnectLimit = 48;
    const int timeOut = 5000;
    const int writeReadTimeOut = 500;

    string url;
    string filePath;
    Action callBack;

    HttpWebRequest headRequest = null;
    WebResponse headResponse = null;

    bool stop = false;

    string localTempFile { get { return filePath + ".temp"; } }

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
        headRequest = HttpWebRequest.Create(url) as HttpWebRequest;

        if (headRequest.ServicePoint.ConnectionLimit < MaxConnectLimit)
        {
            headRequest.ServicePoint.ConnectionLimit = MaxConnectLimit;
        }

        headRequest.Method = WebRequestMethods.Http.Head; // Only the header info, not full file!
        headRequest.ServicePoint.Expect100Continue = false;
        headRequest.Timeout = timeOut;
        bool headRequestOk = false; //是否支持断点续传

        headRequest.BeginGetResponse(OnGotHeadFile, headRequest);

        int tick1 = 0;
        try
        {
            headRequest.BeginGetResponse( //改为异步的方法
                (x) =>
                {
                    HttpWebResponse head_response = null;
                    try
                    {
                        head_response = (x.AsyncState as HttpWebRequest).EndGetResponse(x) as HttpWebResponse;
                        mRemoteLastModified = head_response.LastModified;
                        mRemoteFileSize = head_response.ContentLength;
                        if (head_response.Headers["Accept-Ranges"] != null)
                        {
                            string s = head_response.Headers["Accept-Ranges"];
                            if (s == "none")
                            {
                                isAcceptRange = false;
                            }
                        }
                        System.Threading.Interlocked.Add(ref RemoteFile.TotalRemoteFileSize, mRemoteFileSize);
                        headRequestOk = true;
                    }
                    catch (Exception ex)
                    {
                        DebugEx.LogWarning("ERROR: " + ex);
                    }
                    finally
                    {
                        if (head_response != null)
                        {
                            head_response.Close();
                            head_response = null;
                        }
                        headRequest.Abort();
                    }

                }, headRequest);

            tick1 = System.Environment.TickCount;
        }
        catch (WebException webEx)
        {
            yield break;
        }
        catch (System.Exception e)
        {
            yield break;
        }

    }

    private void OnGotHeadFile(IAsyncResult _result)
    {
        headResponse = headRequest.EndGetResponse(_result);

        FileExtersion.CreateDirectory(localTempFile);

        var isAcceptRange = true;

        HttpWebResponse head_response = null;
        try
        {
            head_response = (x.AsyncState as HttpWebRequest).EndGetResponse(x) as HttpWebResponse;
            mRemoteLastModified = head_response.LastModified;
            mRemoteFileSize = head_response.ContentLength;
            if (head_response.Headers["Accept-Ranges"] != null)
            {
                string s = head_response.Headers["Accept-Ranges"];
                if (s == "none")
                {
                    isAcceptRange = false;
                }
            }
            System.Threading.Interlocked.Add(ref RemoteFile.TotalRemoteFileSize, mRemoteFileSize);
            headRequestOk = true;
        }
        catch (Exception ex)
        {
            DebugEx.LogWarning("ERROR: " + ex);
        }
        finally
        {
            if (head_response != null)
            {
                head_response.Close();
                head_response = null;
            }
            headRequest.Abort();
        }

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
