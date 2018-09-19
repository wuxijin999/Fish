//#define UseWebClient

using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.ComponentModel;
using System;

public class RemoteFile
{
    static bool m_ProcessErroring = false;
    public static bool processErroring
    {
        get { return m_ProcessErroring; }
    }

    public static int MaxConnectLimit = 48;
    static int gDownloadIsRunningCount;
    public static int DownloadIsRunningCount
    {
        get { return gDownloadIsRunningCount; }
    }

    public static int gStartTickcount = 0;
    static long gTotalDownloadSize = 0L;  //已下载的字节数
    static object lockObj = new object();
    public static long TotalDownloadSize
    {
        get
        {
            return System.Threading.Interlocked.Read(ref gTotalDownloadSize);
        }
        set
        {
            gTotalDownloadSize = value;
        }
    }

    static float downloadSpeedRef = 0f; //字节/秒
    static long downloadSizeRef = 0L;

    public static string DownloadSpeed
    {
        get
        {
            float speed = downloadSpeedRef;
            if (RemoteFile.gStartTickcount != 0)
            {
                float second = Mathf.Abs(System.Environment.TickCount - RemoteFile.gStartTickcount) / 1000f;
                if (second > 1f || (downloadSpeedRef <= 0.1f && TotalDownloadSize > 0))
                {
                    if (second > 0f)
                    {
                        var delta = TotalDownloadSize - downloadSizeRef;
                        downloadSizeRef = TotalDownloadSize;

                        speed = (delta / second + downloadSpeedRef) * 0.5f;
                        downloadSpeedRef = speed;
                        RemoteFile.gStartTickcount = System.Environment.TickCount;
                    }
                }
            }

            if (speed > 1048576f)
            {
                return StringUtil.Contact((speed / 1048576f).ToString("f1"), " M/S");
            }
            else if (speed > 1024f)
            {
                return StringUtil.Contact((speed / 1024f).ToString("f1"), " KB/S");
            }
            else
            {
                return StringUtil.Contact(speed.ToString("f1"), " B/S");
            }
        }
    }

    AssetVersion assetVersion;
    protected string mRemoteFile;
    protected string localFile;

    protected string mLocalFileTemp; //临时文件

    protected System.DateTime mRemoteLastModified;
    protected System.DateTime mLocalLastModified;
    protected long mRemoteFileSize = 0;
    public static long TotalRemoteFileSize = 0L;

    const int bufferSize = 8192;
    byte[] buff;

    protected HttpWebResponse mAsynchResponse = null;

    Action<bool, AssetVersion> onCompleted;

    protected bool mHadError = false;
    public bool HaveError
    {
        get { return this.mHadError; }
    }

    bool m_Done = false;
    public bool done
    {
        get { return this.m_Done; }
        private set
        {
            this.m_Done = value;
            if (value)
            {
                if (this.onCompleted != null)
                {
                    this.onCompleted(!this.HaveError, this.assetVersion);
                    this.onCompleted = null;
                }
            }
        }
    }

    int read_Stream_startTickcount = 0; //请求操作开始,用于超时判断
    int timeOut = 5000; //超时时间

    public static void Prepare()
    {
        gStartTickcount = System.Environment.TickCount;
        TotalDownloadSize = 0L;
        downloadSpeedRef = 0f;
        downloadSizeRef = 0L;
    }

    public RemoteFile(string remoteFile, string _localFile, AssetVersion _assetVersion)
    {
        this.mRemoteFile = remoteFile;
        this.localFile = _localFile;
        this.assetVersion = _assetVersion;
    }

    ~RemoteFile()
    {
        if (fs != null)
        {
            fs.Close();
            fs = null;
        }
        if (inStream != null)
        {
            inStream.Close();
            inStream = null;
        }
        mHadError = false;
    }

    public void Reset()
    {
        this.mLocalFileTemp = ""; //临时文件
        this.mRemoteLastModified = DateTime.MinValue;
        this.mLocalLastModified = DateTime.MinValue;
        this.mRemoteFileSize = 0;
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
        this.mHadError = false;
    }


    void MakeSureDirectory(string filePath)
    {
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    bool Move(string sourceFile, string destFile)
    {
        bool ret = true;
        try
        {
            File.Move(sourceFile, destFile);
        }
        catch (Exception ex)
        {
            DebugEx.LogError(ex.Message);
            ret = false;
        }
        return ret;
    }

    public IEnumerator DownloadRemoteFile(Action<bool, AssetVersion> _onCompleted)
    {
        this.done = false;
        this.onCompleted = _onCompleted;

        while (gDownloadIsRunningCount >= MaxConnectLimit)
        {
            //超过并发数时,先等待
            yield return null;
        }

        while (this.assetVersion.extension == ".manifest" && !AssetVersionUtility.GetAssetVersion(this.assetVersion.relativePath.Replace(".manifest", "")).localValid)
        {
            yield return null;
        }

        gDownloadIsRunningCount++;
        this.mHadError = false;
        this.fileWriteState = FileWriteState.None;
        this.mLocalFileTemp = this.localFile + ".tmp";  //先下载为临时文件

        MakeSureDirectory(this.mLocalFileTemp);   //确保文件写入目录存在

        this.mLocalLastModified = DateTime.MinValue;
        long localFileSize = 0L;
        this.mLocalLastModified = File.GetLastWriteTime(this.mLocalFileTemp);
        HttpWebRequest headRequest = (HttpWebRequest)System.Net.WebRequest.Create(this.mRemoteFile);
        if (headRequest.ServicePoint.ConnectionLimit < RemoteFile.MaxConnectLimit)
        {
            headRequest.ServicePoint.ConnectionLimit = RemoteFile.MaxConnectLimit;
        }
        headRequest.Method = "HEAD"; // Only the header info, not full file!
        headRequest.ServicePoint.Expect100Continue = false;
        headRequest.Timeout = 3000;
        bool isAcceptRange = true;
        bool headRequestOk = false; //是否支持断点续传

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
                        this.mRemoteLastModified = head_response.LastModified;
                        this.mRemoteFileSize = head_response.ContentLength;
                        if (head_response.Headers["Accept-Ranges"] != null)
                        {
                            string s = head_response.Headers["Accept-Ranges"];
                            if (s == "none")
                            {
                                isAcceptRange = false;
                            }
                        }
                        System.Threading.Interlocked.Add(ref RemoteFile.TotalRemoteFileSize, this.mRemoteFileSize);
                        headRequestOk = true;
                    }
                    catch (Exception ex)
                    {
                        DebugEx.LogWarning("ERROR: " + ex);
                        this.mHadError = true;
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
            DebugEx.LogWarning("<color=red>Request File Head ERROR: " + this.mRemoteFile + "</color>");
            DebugEx.LogWarning("ERROR: " + webEx);
            this.mHadError = true;
            gDownloadIsRunningCount--;
            this.done = true;
            yield break;
        }
        catch (System.Exception e)
        {
            DebugEx.LogWarning("<color=red>Request File Head ERROR: " + this.mRemoteFile + "</color>");
            DebugEx.LogWarning("ERROR: " + e);
            this.mHadError = true;
            gDownloadIsRunningCount--;
            this.done = true;
            yield break;
        }

        while (!headRequestOk && !this.mHadError)
        {
            if (processErroring)
            {
                this.mHadError = true;
                break;
            }
            float dur = System.Environment.TickCount - tick1;
            if (dur > this.timeOut)
            {
                DebugEx.LogWarningFormat("获取远程文件{0} 信息超时!", this.mRemoteFile);
                this.mHadError = true;
                break;
            }
            yield return null;
        }

        if (this.mHadError)
        {
            DebugEx.LogWarningFormat("获取远程文件{0} 信息失败!", this.mRemoteFile);
            headRequest.Abort();
            this.done = true;
            gDownloadIsRunningCount--;
            yield break;
        }

        //判断是否有已经下载部分的临时文件
        if (File.Exists(this.mLocalFileTemp))
        { // This will not work in web player!
            //判断是否断点续传, 依据临时文件是否存在,以及修改时间是否小于服务器文件时间
            localFileSize = (File.Exists(this.mLocalFileTemp)) ? (new FileInfo(this.mLocalFileTemp)).Length : 0L;
            bool outDated = this.IsOutdated;
            if (localFileSize == this.mRemoteFileSize && !outDated)
            {
                gDownloadIsRunningCount--;
                this.done = true;
                this.mHadError = !Move(this.mLocalFileTemp, this.localFile);//把临时文件改名为正式文件
                yield break; // We already have the file, early out
            }
            else if (localFileSize > this.mRemoteFileSize || outDated)
            {
                if (!outDated) DebugEx.LogWarning("Local file is larger than remote file, but not outdated. PANIC!");
                if (outDated)
                {
                    DebugEx.LogWarning(this.mLocalFileTemp + " Local file is outdated, deleting");
                }
                try
                {
                    if (File.Exists(this.mLocalFileTemp))
                        File.Delete(this.mLocalFileTemp);
                }
                catch (System.Exception e)
                {
                    DebugEx.LogWarning("<color=red>Could not delete local file</color>");
                    DebugEx.LogError(e);
                }

                while (File.Exists(this.mLocalFileTemp))
                {
                    yield return null;
                }

                localFileSize = 0;
            }
        }

        if (this.mHadError)
        {
            gDownloadIsRunningCount--;
            this.done = true;
            yield break;
        }

        if (File.Exists(this.localFile))
        {
            try
            {
                if (File.Exists(this.localFile))
                    File.Delete(this.localFile);
            }
            catch (System.Exception e)
            {
                DebugEx.LogWarning("<color=red>Could not delete local file</color>");
                DebugEx.LogWarning(e);
            }
            while (File.Exists(this.localFile))
            {
                yield return null;
            }
        }

        HttpWebRequest request = null;
        try
        {
            request = (HttpWebRequest)HttpWebRequest.Create(this.mRemoteFile);
            if (request.ServicePoint.ConnectionLimit < RemoteFile.MaxConnectLimit)
            {
                request.ServicePoint.ConnectionLimit = RemoteFile.MaxConnectLimit;
            }
            request.ServicePoint.Expect100Continue = false;
            request.Timeout = 3000;
            if (localFileSize != 0L && isAcceptRange)
            {
                //CatDebugger.Log("文件{0} 开始断点续传 from {1} to {2}", mRemoteFile, localFileSize, mRemoteFileSize);
                request.AddRange((int)localFileSize, (int)this.mRemoteFileSize - 1);
            }
            request.Method = WebRequestMethods.Http.Get;
            request.BeginGetResponse(this.AsynchCallback, request);
            tick1 = System.Environment.TickCount;
        }
        catch (System.Exception ex)
        {
            DebugEx.LogWarning("BeginGetResponse exception: " + ex.Message);
            DebugEx.LogWarning(ex);
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            this.mHadError = true;
            this.done = true;
            gDownloadIsRunningCount--;
            yield break;
        }

        while (this.mAsynchResponse == null && !this.mHadError)
        { // Wait for asynch to finish
            if (processErroring)
            {
                this.mHadError = true;
                break;
            }
            float dur = System.Environment.TickCount - tick1;
            if (dur > this.timeOut)
            {
                DebugEx.LogWarningFormat("下载远程文件{0} 超时!", this.mRemoteFile);
                this.mHadError = true;
                break;
            }
            yield return null;
        }

        if (this.mHadError)
        {
            DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 下载失败! ", this.localFile);
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            if (this.mAsynchResponse != null)
            {
                this.mAsynchResponse.Close();
                this.mAsynchResponse = null;
            }
            this.done = true;
            gDownloadIsRunningCount--;
            yield break;
        }

        try
        {
            this.inStream = this.mAsynchResponse.GetResponseStream();
            this.fs = new FileStream(this.mLocalFileTemp, (localFileSize > 0) ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            if (this.buff == null)
            {
                this.buff = new byte[bufferSize];
            }
            this.fileWriteState = FileWriteState.Writting;
            this.inStream.BeginRead(this.buff, 0, bufferSize, this.ReadDataCallback, null);
            this.read_Stream_startTickcount = System.Environment.TickCount;
        }
        catch (Exception ex)
        {
            DebugEx.LogWarning("<color=red>ERROR: " + this.mRemoteFile + "</color>");
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
                    DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 读取超时{1}!", this.mRemoteFile, dura);
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
            FileInfo localTempFileInfo = new FileInfo(this.mLocalFileTemp);
            if (localTempFileInfo.Exists)
            { //临时文件存在,需要判断大小是否一致
              //判断临时文件和远程文件size是否一致
                if (localTempFileInfo.Length != this.mRemoteFileSize && this.mRemoteFileSize != 0L)
                {
                    this.mHadError = true;
                    DebugEx.LogError(string.Format(this.localFile + " 下载完成后, 但是大小{0} 和远程文件不一致 {1}", localTempFileInfo.Length, this.mRemoteFileSize));
                }
                else
                {  //大小一致 
                    this.mHadError = !Move(this.mLocalFileTemp, this.localFile);//把临时文件改名为正式文件
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

    bool IsOutdated
    {
        get
        {
            if (File.Exists(this.mLocalFileTemp))
                return this.mRemoteLastModified > this.mLocalLastModified;
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
            lock (lockObj)
            {
                gTotalDownloadSize += read;
            }
            if (read > 0)
            {
                this.fs.Write(this.buff, 0, read);
                this.fs.Flush();
                this.inStream.BeginRead(this.buff, 0, bufferSize, new AsyncCallback(ReadDataCallback), null);
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

