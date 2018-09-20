//#define UseWebClient

using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System;

public class FileDownLoad
{
    const int BUFFER_SIZE = 8192;
    public const int MaxConnectLimit = 12;

    protected string remoteFile;
    protected string localFile;
    protected string tempFile { get { return StringUtil.Contact(localFile, ".temp"); } } //临时文件

    protected DateTime remoteLastModifiedTime;
    protected DateTime localLastModifiedTime;

    protected long remoteFileSize = 0;

    int read_Stream_startTickcount = 0; //请求操作开始,用于超时判断
    int timeOut = 5000; //超时时间

    byte[] buff;

    protected HttpWebResponse mAsynchResponse = null;

    Action<bool, AssetVersion> onCompleted;

    public FileDownLoad(string remoteFile, string _localFile)
    {
        this.remoteFile = remoteFile;
        this.localFile = _localFile;
    }

    ~FileDownLoad()
    {
        Dispose();
    }

    public void Dispose()
    {

    }

    private void ProcessTaskResult(bool ok, string description)
    {

    }

    void CreateDirectory(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    bool MoveFile(string source, string dest)
    {
        try
        {
            File.Move(source, dest);
        }
        catch (Exception ex)
        {
            DebugEx.LogError(ex.Message);
            return false;
        }

        return true;
    }


    class HeadFileResult
    {
        public bool ok = false;
        public bool isAcceptRange = true;
        public HttpWebRequest request;
    }

    private void Prepare()
    {
        CreateDirectory(this.tempFile);   //确保文件写入目录存在
        this.localLastModifiedTime = File.GetLastWriteTime(this.tempFile);
    }

    private void BeginGetHeadFile()
    {
        var request = (HttpWebRequest)System.Net.WebRequest.Create(this.remoteFile);
        if (request.ServicePoint.ConnectionLimit < RemoteFile.MaxConnectLimit)
        {
            request.ServicePoint.ConnectionLimit = RemoteFile.MaxConnectLimit;
        }

        request.Method = "HEAD"; // Only the header info, not full file!
        request.ServicePoint.Expect100Continue = false;
        request.Timeout = 3000;

        int tick1 = 0;
        try
        {
            request.BeginGetResponse(OnGetHeadFile, null);
            tick1 = System.Environment.TickCount;
        }
        catch (WebException webEx)
        {
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            DebugEx.LogWarning("<color=red>Request File Head ERROR: " + this.remoteFile + "</color>");
            DebugEx.LogWarning("ERROR: " + webEx);
        }
        catch (System.Exception e)
        {
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            DebugEx.LogWarning("<color=red>Request File Head ERROR: " + this.remoteFile + "</color>");
            DebugEx.LogWarning("ERROR: " + e);
        }
    }

    private void OnGetHeadFile(IAsyncResult result)
    {
        HttpWebResponse response = null;
        var acceptRange = false;

        try
        {
            response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            this.remoteLastModifiedTime = response.LastModified;
            this.remoteFileSize = response.ContentLength;
            System.Threading.Interlocked.Add(ref RemoteFile.TotalRemoteFileSize, this.remoteFileSize);

            if (response.Headers["Accept-Ranges"] != null)
            {
                var s = response.Headers["Accept-Ranges"];
                if (s == "none")
                {
                    acceptRange = true;
                }
            }

            BeginGetFile(acceptRange);
        }
        catch (Exception ex)
        {
            DebugEx.LogError("ERROR: " + ex);
        }
        finally
        {
            if (response != null)
            {
                response.Close();
                response = null;
            }
        }
    }

    private void BeginGetFile(bool acceptRange)
    {
        var tempFileSize = 0L;
        if (File.Exists(this.tempFile))
        {
            tempFileSize = new FileInfo(this.tempFile).Length;
            var outDated = this.remoteLastModifiedTime > this.localLastModifiedTime;
            if (outDated)
            {
                File.Delete(this.tempFile);
            }
            else
            {
                if (tempFileSize == this.remoteFileSize)
                {
                    MoveFile(this.tempFile, this.localFile);
                    //完成

                    return;
                }
                else if (tempFileSize > this.remoteFileSize)
                {
                    File.Delete(this.tempFile);
                }
            }
        }

        if (File.Exists(this.localFile))
        {
            try
            {
                File.Delete(this.localFile);
            }
            catch (Exception e)
            {
                DebugEx.LogWarning(e);
            }
        }

        HttpWebRequest request = null;
        try
        {
            request = (HttpWebRequest)HttpWebRequest.Create(this.remoteFile);
            if (request.ServicePoint.ConnectionLimit < RemoteFile.MaxConnectLimit)
            {
                request.ServicePoint.ConnectionLimit = RemoteFile.MaxConnectLimit;
            }

            request.ServicePoint.Expect100Continue = false;
            request.Timeout = 3000;
            if (tempFileSize != 0L && acceptRange)
            {
                request.AddRange((int)tempFileSize, (int)this.remoteFileSize - 1);
            }

            request.Method = WebRequestMethods.Http.Get;
            request.BeginGetResponse(this.OnGetFile, request);
        }
        catch (System.Exception ex)
        {
            DebugEx.LogError(ex);
            if (request != null)
            {
                request.Abort();
                request = null;
            }
        }
    }

    private void OnGetFile(IAsyncResult result)
    {
        HttpWebResponse response = null;
        Stream stream = null;
        FileStream fileStream = null;
        try
        {
            response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            stream = response.GetResponseStream();
            var teamFileSize = File.Exists(this.tempFile) ? new FileInfo(this.tempFile).Length : 0;
            fileStream = new FileStream(this.tempFile, (teamFileSize > 0) ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BeginReadFile(stream, fileStream);
            this.read_Stream_startTickcount = System.Environment.TickCount;
        }
        catch (Exception ex)
        {
            DebugEx.LogError(ex);
            if (stream != null)
            {
                stream.Close();
            }

            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    class FileStreams
    {
        public Stream stream;
        public FileStream fileStream;
    }

    private void BeginReadFile(Stream stream, FileStream fileStream)
    {
        if (this.buff == null)
        {
            this.buff = new byte[BUFFER_SIZE];
        }

        var streams = new FileStreams();
        streams.stream = stream;
        streams.fileStream = fileStream;
        stream.BeginRead(this.buff, 0, BUFFER_SIZE, this.OnEndReadFlush, streams);
    }

    private void OnEndReadFlush(IAsyncResult result)
    {
        var streams = result.AsyncState as FileStreams;
        var read = streams.stream.EndRead(result);
        if (read > 0)
        {
            streams.fileStream.Write(this.buff, 0, read);
            streams.fileStream.Flush();
            streams.stream.BeginRead(this.buff, 0, BUFFER_SIZE, OnEndReadFlush, streams);
        }
        else
        {
            streams.stream.Close();
            streams.fileStream.Close();
            OnReadFileEnd();
        }
    }

    private void OnReadFileEnd()
    {
        if (File.Exists(this.tempFile))
        {
            var fileInfo = new FileInfo(this.tempFile);
            if (this.remoteFileSize != 0L && fileInfo.Length == this.remoteFileSize)
            {
                MoveFile(this.tempFile, this.localFile);
            }
        }
        else
        {
            //处理结束
        }
    }


}

