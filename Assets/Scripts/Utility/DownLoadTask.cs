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

    public void Reset()
    {
        this.remoteLastModifiedTime = DateTime.MinValue;
        this.localLastModifiedTime = DateTime.MinValue;
        this.remoteFileSize = 0;

        if (this.fs != null)
        {
            this.fs.Close();
            this.fs = null;
        }
        if (this.inStream != null)
        {
            this.inStream.Close();
            this.inStream = null;
        }
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

    }

    private void BeginReadFile()
    {

    }

    private  void OnEndReadFlush()
    {

    }


    public IEnumerator DownloadRemoteFile(Action<bool, AssetVersion> _onCompleted)
    {
        this.onCompleted = _onCompleted;

        this.fileWriteState = FileWriteState.None;
        CreateDirectory(this.tempFile);   //确保文件写入目录存在
        this.localLastModifiedTime = File.GetLastWriteTime(this.tempFile);

        long localFileSize = 0L;

        try
        {
            this.inStream = this.mAsynchResponse.GetResponseStream();
            this.fs = new FileStream(this.tempFile, (localFileSize > 0) ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            if (this.buff == null)
            {
                this.buff = new byte[BUFFER_SIZE];
            }
            this.fileWriteState = FileWriteState.Writting;
            this.inStream.BeginRead(this.buff, 0, BUFFER_SIZE, this.ReadDataCallback, null);
            this.read_Stream_startTickcount = System.Environment.TickCount;
        }
        catch (Exception ex)
        {
            DebugEx.LogWarning("<color=red>ERROR: " + this.remoteFile + "</color>");
            DebugEx.LogWarning(ex);
            if (this.inStream != null)
            {
                this.inStream.Close();
                this.inStream = null;
            }
            if (this.fs != null)
            {
                this.fs.Close();
                this.fs = null;
            }
            if (this.mAsynchResponse != null)
            {
                this.mAsynchResponse.Close();
                this.mAsynchResponse = null;
            }
            this.mHadError = true;
            this.fileWriteState = FileWriteState.Error;
        }

        while (this.fileWriteState == FileWriteState.Writting)
        {
            if (processErroring)
            {
                this.fileWriteState = FileWriteState.Error;
                break;
            }
            if (downloadSpeedRef == 0)
            {
                int dura = System.Environment.TickCount - this.read_Stream_startTickcount;
                if (dura > this.timeOut)
                {
                    this.fileWriteState = FileWriteState.Timeout;
                    DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 读取超时{1}!", this.remoteFile, dura);
                    break;
                }
            }

            yield return null;
        }

        if (request != null)
        {
            request.Abort();
            request = null;
        }

        if (this.fileWriteState == FileWriteState.Error || this.fileWriteState == FileWriteState.Timeout)
        {
            DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 下载失败! ", this.localFile);
            if (this.mAsynchResponse != null)
            {
                this.mAsynchResponse.Close();
                this.mAsynchResponse = null;
            }
            this.mHadError = true;
            this.done = true;
            gDownloadIsRunningCount--;
            yield break;
        }
        try
        {
            FileInfo localTempFileInfo = new FileInfo(this.tempFile);
            if (localTempFileInfo.Exists)
            { //临时文件存在,需要判断大小是否一致
              //判断临时文件和远程文件size是否一致
                if (localTempFileInfo.Length != this.remoteFileSize && this.remoteFileSize != 0L)
                {
                    this.mHadError = true;
                    DebugEx.LogError(string.Format(this.localFile + " 下载完成后, 但是大小{0} 和远程文件不一致 {1}", localTempFileInfo.Length, this.remoteFileSize));
                }
                else
                {  //大小一致 
                    this.mHadError = !MoveFile(this.tempFile, this.localFile);//把临时文件改名为正式文件
                }
                gDownloadIsRunningCount--;
                this.done = true;
            }
            else
            { //临时文件不存在
                this.mHadError = true;
                gDownloadIsRunningCount--;
                this.done = true;
            }
        }
        catch (Exception ex)
        {
            DebugEx.LogError(ex);
            this.mHadError = true;
        }
    }

    bool IsOutdated {
        get {
            if (File.Exists(this.tempFile))
                return this.remoteLastModifiedTime > this.localLastModifiedTime;
            return false;
        }
    }

    enum FileWriteState
    {
        None,
        Writting,
        Completed,
        Error,
        Timeout,
    }

    Stream inStream;
    FileStream fs;
    FileWriteState fileWriteState = FileWriteState.None;  //下载文件写入状态
    void ReadDataCallback(IAsyncResult ar)
    {
        try
        {
            int read = this.inStream.EndRead(ar);
            if (read > 0)
            {
                this.fs.Write(this.buff, 0, read);
                this.fs.Flush();
                this.inStream.BeginRead(this.buff, 0, BUFFER_SIZE, new AsyncCallback(ReadDataCallback), null);
                this.read_Stream_startTickcount = System.Environment.TickCount;
            }
            else
            {
                this.fs.Close();
                this.fs = null;
                this.inStream.Close();
                this.inStream = null;
                this.mAsynchResponse.Close();
                this.mAsynchResponse = null;
                this.fileWriteState = FileWriteState.Completed;
            }
        }
        catch (Exception ex)
        {
            if (!processErroring)
            {
                DebugEx.LogWarning(ex);
                DebugEx.LogWarning("ReadDataCallback 异常信息: " + ex.Message);
            }
            if (this.fs != null)
            {
                this.fs.Close();
                this.fs = null;
            }
            if (this.inStream != null)
            {
                this.inStream.Close();
                this.inStream = null;
            }
            this.fileWriteState = FileWriteState.Error;
        }
    }

    protected void AsynchCallback(IAsyncResult result)
    {
        try
        {
            if (result == null)
            {
                DebugEx.LogError("Asynch result is null!");
                this.mHadError = true;
            }

            HttpWebRequest webRequest = (HttpWebRequest)result.AsyncState;
            if (webRequest == null)
            {
                DebugEx.LogError("Could not cast to web request");
                this.mHadError = true;
            }

            this.mAsynchResponse = webRequest.EndGetResponse(result) as HttpWebResponse;
            if (this.mAsynchResponse == null)
            {
                DebugEx.LogError("Asynch response is null!");
                this.mHadError = true;
            }
        }
        catch (Exception ex)
        {
            this.mHadError = true;
            DebugEx.LogWarning(ex);
            DebugEx.LogWarning("[RemoteFile] AsynchCallback 异常: " + ex.Message);
        }
    }
}

