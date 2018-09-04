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
        get { return mHadError; }
    }

    bool m_Done = false;
    public bool done
    {
        get { return m_Done; }
        private set
        {
            m_Done = value;
            if (value)
            {
                if (onCompleted != null)
                {
                    onCompleted(!HaveError, assetVersion);
                    onCompleted = null;
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
        mRemoteFile = remoteFile;
        localFile = _localFile;
        assetVersion = _assetVersion;
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
        mLocalFileTemp = ""; //临时文件
        mRemoteLastModified = DateTime.MinValue;
        mLocalLastModified = DateTime.MinValue;
        mRemoteFileSize = 0;
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
        done = false;
        onCompleted = _onCompleted;

        while (gDownloadIsRunningCount >= MaxConnectLimit)
        {
            //超过并发数时,先等待
            yield return null;
        }

        while (assetVersion.extension == ".manifest" && !AssetVersionUtility.GetAssetVersion(assetVersion.relativePath.Replace(".manifest", "")).localValid)
        {
            yield return null;
        }

        gDownloadIsRunningCount++;
        mHadError = false;
        fileWriteState = FileWriteState.None;
        mLocalFileTemp = localFile + ".tmp";  //先下载为临时文件

        MakeSureDirectory(mLocalFileTemp);   //确保文件写入目录存在

        mLocalLastModified = DateTime.MinValue;
        long localFileSize = 0L;
        mLocalLastModified = File.GetLastWriteTime(mLocalFileTemp);
        HttpWebRequest headRequest = (HttpWebRequest)System.Net.WebRequest.Create(mRemoteFile);
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
                        mHadError = true;
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
            DebugEx.LogWarning("<color=red>Request File Head ERROR: " + mRemoteFile + "</color>");
            DebugEx.LogWarning("ERROR: " + webEx);
            mHadError = true;
            gDownloadIsRunningCount--;
            done = true;
            yield break;
        }
        catch (System.Exception e)
        {
            DebugEx.LogWarning("<color=red>Request File Head ERROR: " + mRemoteFile + "</color>");
            DebugEx.LogWarning("ERROR: " + e);
            mHadError = true;
            gDownloadIsRunningCount--;
            done = true;
            yield break;
        }

        while (!headRequestOk && !mHadError)
        {
            if (processErroring)
            {
                mHadError = true;
                break;
            }
            float dur = System.Environment.TickCount - tick1;
            if (dur > timeOut)
            {
                DebugEx.LogWarningFormat("获取远程文件{0} 信息超时!", mRemoteFile);
                mHadError = true;
                break;
            }
            yield return null;
        }

        if (mHadError)
        {
            DebugEx.LogWarningFormat("获取远程文件{0} 信息失败!", mRemoteFile);
            headRequest.Abort();
            done = true;
            gDownloadIsRunningCount--;
            yield break;
        }

        //判断是否有已经下载部分的临时文件
        if (File.Exists(mLocalFileTemp))
        { // This will not work in web player!
            //判断是否断点续传, 依据临时文件是否存在,以及修改时间是否小于服务器文件时间
            localFileSize = (File.Exists(mLocalFileTemp)) ? (new FileInfo(mLocalFileTemp)).Length : 0L;
            bool outDated = IsOutdated;
            if (localFileSize == mRemoteFileSize && !outDated)
            {
                gDownloadIsRunningCount--;
                done = true;
                mHadError = !Move(mLocalFileTemp, localFile);//把临时文件改名为正式文件
                yield break; // We already have the file, early out
            }
            else if (localFileSize > mRemoteFileSize || outDated)
            {
                if (!outDated) DebugEx.LogWarning("Local file is larger than remote file, but not outdated. PANIC!");
                if (outDated)
                {
                    DebugEx.LogWarning(mLocalFileTemp + " Local file is outdated, deleting");
                }
                try
                {
                    if (File.Exists(mLocalFileTemp))
                        File.Delete(mLocalFileTemp);
                }
                catch (System.Exception e)
                {
                    DebugEx.LogWarning("<color=red>Could not delete local file</color>");
                    DebugEx.LogError(e);
                }

                while (File.Exists(mLocalFileTemp))
                {
                    yield return null;
                }

                localFileSize = 0;
            }
        }

        if (mHadError)
        {
            gDownloadIsRunningCount--;
            done = true;
            yield break;
        }

        if (File.Exists(localFile))
        {
            try
            {
                if (File.Exists(localFile))
                    File.Delete(localFile);
            }
            catch (System.Exception e)
            {
                DebugEx.LogWarning("<color=red>Could not delete local file</color>");
                DebugEx.LogWarning(e);
            }
            while (File.Exists(localFile))
            {
                yield return null;
            }
        }

        HttpWebRequest request = null;
        try
        {
            request = (HttpWebRequest)HttpWebRequest.Create(mRemoteFile);
            if (request.ServicePoint.ConnectionLimit < RemoteFile.MaxConnectLimit)
            {
                request.ServicePoint.ConnectionLimit = RemoteFile.MaxConnectLimit;
            }
            request.ServicePoint.Expect100Continue = false;
            request.Timeout = 3000;
            if (localFileSize != 0L && isAcceptRange)
            {
                //CatDebugger.Log("文件{0} 开始断点续传 from {1} to {2}", mRemoteFile, localFileSize, mRemoteFileSize);
                request.AddRange((int)localFileSize, (int)mRemoteFileSize - 1);
            }
            request.Method = WebRequestMethods.Http.Get;
            request.BeginGetResponse(AsynchCallback, request);
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
            mHadError = true;
            done = true;
            gDownloadIsRunningCount--;
            yield break;
        }

        while (mAsynchResponse == null && !mHadError)
        { // Wait for asynch to finish
            if (processErroring)
            {
                mHadError = true;
                break;
            }
            float dur = System.Environment.TickCount - tick1;
            if (dur > timeOut)
            {
                DebugEx.LogWarningFormat("下载远程文件{0} 超时!", mRemoteFile);
                mHadError = true;
                break;
            }
            yield return null;
        }

        if (mHadError)
        {
            DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 下载失败! ", localFile);
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            if (mAsynchResponse != null)
            {
                mAsynchResponse.Close();
                mAsynchResponse = null;
            }
            done = true;
            gDownloadIsRunningCount--;
            yield break;
        }

        try
        {
            inStream = mAsynchResponse.GetResponseStream();
            fs = new FileStream(mLocalFileTemp, (localFileSize > 0) ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            if (buff == null)
            {
                buff = new byte[bufferSize];
            }
            fileWriteState = FileWriteState.Writting;
            inStream.BeginRead(buff, 0, bufferSize, ReadDataCallback, null);
            read_Stream_startTickcount = System.Environment.TickCount;
        }
        catch (Exception ex)
        {
            DebugEx.LogWarning("<color=red>ERROR: " + mRemoteFile + "</color>");
            DebugEx.LogWarning(ex);
            if (inStream != null)
            {
                inStream.Close();
                inStream = null;
            }
            if (fs != null)
            {
                fs.Close();
                fs = null;
            }
            if (mAsynchResponse != null)
            {
                mAsynchResponse.Close();
                mAsynchResponse = null;
            }
            mHadError = true;
            fileWriteState = FileWriteState.Error;
        }

        while (fileWriteState == FileWriteState.Writting)
        {
            if (processErroring)
            {
                fileWriteState = FileWriteState.Error;
                break;
            }
            if (downloadSpeedRef == 0)
            {
                int dura = System.Environment.TickCount - read_Stream_startTickcount;
                if (dura > timeOut)
                {
                    fileWriteState = FileWriteState.Timeout;
                    DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 读取超时{1}!", mRemoteFile, dura);
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

        if (fileWriteState == FileWriteState.Error || fileWriteState == FileWriteState.Timeout)
        {
            DebugEx.LogWarningFormat("[RemoteFile] 远程文件{0} 下载失败! ", localFile);
            if (mAsynchResponse != null)
            {
                mAsynchResponse.Close();
                mAsynchResponse = null;
            }
            mHadError = true;
            done = true;
            gDownloadIsRunningCount--;
            yield break;
        }
        try
        {
            FileInfo localTempFileInfo = new FileInfo(mLocalFileTemp);
            if (localTempFileInfo.Exists)
            { //临时文件存在,需要判断大小是否一致
              //判断临时文件和远程文件size是否一致
                if (localTempFileInfo.Length != mRemoteFileSize && mRemoteFileSize != 0L)
                {
                    mHadError = true;
                    DebugEx.LogError(string.Format(localFile + " 下载完成后, 但是大小{0} 和远程文件不一致 {1}", localTempFileInfo.Length, mRemoteFileSize));
                }
                else
                {  //大小一致 
                    mHadError = !Move(mLocalFileTemp, localFile);//把临时文件改名为正式文件
                }
                gDownloadIsRunningCount--;
                done = true;
            }
            else
            { //临时文件不存在
                mHadError = true;
                gDownloadIsRunningCount--;
                done = true;
            }
        }
        catch (Exception ex)
        {
            DebugEx.LogError(ex);
            mHadError = true;
        }
    }

    bool IsOutdated
    {
        get
        {
            if (File.Exists(mLocalFileTemp))
                return mRemoteLastModified > mLocalLastModified;
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
            int read = inStream.EndRead(ar);
            lock (lockObj)
            {
                gTotalDownloadSize += read;
            }
            if (read > 0)
            {
                fs.Write(buff, 0, read);
                fs.Flush();
                inStream.BeginRead(buff, 0, bufferSize, new AsyncCallback(ReadDataCallback), null);
                read_Stream_startTickcount = System.Environment.TickCount;
            }
            else
            {
                fs.Close();
                fs = null;
                inStream.Close();
                inStream = null;
                mAsynchResponse.Close();
                mAsynchResponse = null;
                fileWriteState = FileWriteState.Completed;
            }
        }
        catch (Exception ex)
        {
            if (!processErroring)
            {
                DebugEx.LogWarning(ex);
                DebugEx.LogWarning("ReadDataCallback 异常信息: " + ex.Message);
            }
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
            fileWriteState = FileWriteState.Error;
        }
    }

    protected void AsynchCallback(IAsyncResult result)
    {
        try
        {
            if (result == null)
            {
                DebugEx.LogError("Asynch result is null!");
                mHadError = true;
            }

            HttpWebRequest webRequest = (HttpWebRequest)result.AsyncState;
            if (webRequest == null)
            {
                DebugEx.LogError("Could not cast to web request");
                mHadError = true;
            }

            mAsynchResponse = webRequest.EndGetResponse(result) as HttpWebResponse;
            if (mAsynchResponse == null)
            {
                DebugEx.LogError("Asynch response is null!");
                mHadError = true;
            }
        }
        catch (Exception ex)
        {
            mHadError = true;
            DebugEx.LogWarning(ex);
            DebugEx.LogWarning("[RemoteFile] AsynchCallback 异常: " + ex.Message);
        }
    }
}

