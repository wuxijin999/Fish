using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DownLoadAndDiscompressTask : Singleton<DownLoadAndDiscompressTask>
{
    public const int BYTE_PER_KILOBYTE = 1024;
    public const int BYTE_PER_MILLIONBYTE = 1048576;

    public float progress
    {
        get
        {
            return Mathf.Clamp01((float)RemoteFile.TotalDownloadSize / this.totalSize);
        }
    }

    List<AssetVersion> tasks = new List<AssetVersion>();
    public bool isDone { get { return this.step == Step.Completed; } }
    public int totalSize { get; private set; }
    public int totalCount { get; private set; }
    public int okCount { get; private set; }
    public bool restartApp { get; private set; }

    public event Action<Step> downLoadStepChangeEvent;

    System.Action downLoadOkCallBack;

    Step m_Step = Step.None;
    public Step step
    {
        get { return this.m_Step; }
        set
        {
            if (this.m_Step != value)
            {
                this.m_Step = value;

                if (downLoadStepChangeEvent != null)
                {
                    downLoadStepChangeEvent(this.m_Step);
                }
            }
        }
    }

    public void Prepare(List<AssetVersion> downLoadTasks, bool prior, System.Action downLoadOkCallBack)
    {
        this.tasks = downLoadTasks;
        this.downLoadOkCallBack = downLoadOkCallBack;

        this.totalCount = this.tasks.Count;
        this.okCount = 0;
        this.step = Step.DownLoadPrepared;
        this.restartApp = false;
        this.totalSize = 0;

        for (int i = 0; i < this.tasks.Count; i++)
        {
            var task = this.tasks[i];
            this.totalSize += task.size;
#if UNITY_ANDROID
            if (!restartApp && task.GetAssetCategory() == AssetVersion.AssetCategory.Dll)
            {
                restartApp = true;
            }
#endif
        }

        StartDownLoad();
    }

    public void StartDownLoad()
    {
        this.step = Step.DownLoad;
        CoroutineUtil.Instance.Begin(Co_StartDownLoad());
    }

    IEnumerator Co_StartDownLoad()
    {
        RemoteFile.Prepare();

        for (int i = 0; i < this.tasks.Count; i++)
        {
            var assetVersion = this.tasks[i];

            var remoteURL = StringUtil.Contact(VersionUtil.Instance.versionInfo.GetResourcesURL(VersionConfig.Get().branch), "/", assetVersion.relativePath);
            var localURL = StringUtil.Contact(AssetPath.ExternalStorePath, assetVersion.relativePath);

            var remoteFile = new RemoteFile(remoteURL, localURL, assetVersion);
            CoroutineUtil.Instance.Begin(remoteFile.DownloadRemoteFile(this.OnFileDownLoadCompleted));
        }

        while (this.okCount < this.totalCount)
        {
            yield return null;
        }

        this.step = Step.Completed;

        if (this.downLoadOkCallBack != null)
        {
            this.downLoadOkCallBack();
            this.downLoadOkCallBack = null;
        }

        if (this.restartApp)
        {
        }
    }

    private void OnFileDownLoadCompleted(bool ok, AssetVersion assetVersion)
    {
        if (ok)
        {
            this.okCount++;
            assetVersion.localValid = true;
        }
        else
        {
            var remoteURL = StringUtil.Contact(VersionUtil.Instance.versionInfo.GetResourcesURL(VersionConfig.Get().branch), "/", assetVersion.relativePath);
            var localURL = StringUtil.Contact(AssetPath.ExternalStorePath, assetVersion.relativePath);

            var remoteFile = new RemoteFile(remoteURL, localURL, assetVersion);
            CoroutineUtil.Instance.Begin(remoteFile.DownloadRemoteFile(this.OnFileDownLoadCompleted));
        }
    }

    public enum Step
    {
        None,
        DownLoadPrepared,
        DownLoad,
        Completed,
    }

}
