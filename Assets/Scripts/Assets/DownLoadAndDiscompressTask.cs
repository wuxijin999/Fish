﻿using System.Collections;
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
            return Mathf.Clamp01((float)RemoteFile.TotalDownloadSize / totalSize);
        }
    }

    List<AssetVersion> tasks = new List<AssetVersion>();
    public bool isDone { get { return step == Step.Completed; } }
    public int totalSize { get; private set; }
    public int totalCount { get; private set; }
    public int okCount { get; private set; }
    public bool restartApp { get; private set; }

    public event Action<Step> downLoadStepChangeEvent;

    System.Action downLoadOkCallBack;

    Step m_Step = Step.None;
    public Step step
    {
        get { return m_Step; }
        set
        {
            if (m_Step != value)
            {
                m_Step = value;

                if (downLoadStepChangeEvent != null)
                {
                    downLoadStepChangeEvent(m_Step);
                }
            }
        }
    }

    public void Prepare(List<AssetVersion> downLoadTasks, bool prior, System.Action downLoadOkCallBack)
    {
        tasks = downLoadTasks;
        this.downLoadOkCallBack = downLoadOkCallBack;

        totalCount = tasks.Count;
        okCount = 0;
        step = Step.DownLoadPrepared;
        restartApp = false;
        totalSize = 0;

        for (int i = 0; i < tasks.Count; i++)
        {
            var task = tasks[i];
            totalSize += task.size;
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
        step = Step.DownLoad;
        CoroutineUtil.Instance.Coroutine(Co_StartDownLoad());
    }

    IEnumerator Co_StartDownLoad()
    {
        RemoteFile.Prepare();

        for (int i = 0; i < tasks.Count; i++)
        {
            var assetVersion = tasks[i];

            var remoteURL = StringUtil.Contact(VersionUtility.Instance.versionInfo.GetResourcesURL(VersionConfig.Get().branch), "/", assetVersion.relativePath);
            var localURL = StringUtil.Contact(AssetPath.ExternalStorePath, assetVersion.relativePath);

            var remoteFile = new RemoteFile(remoteURL, localURL, assetVersion);
            CoroutineUtil.Instance.Coroutine(remoteFile.DownloadRemoteFile(OnFileDownLoadCompleted));
        }

        while (okCount < totalCount)
        {
            yield return null;
        }

        step = Step.Completed;

        if (downLoadOkCallBack != null)
        {
            downLoadOkCallBack();
            downLoadOkCallBack = null;
        }

        if (restartApp)
        {
        }
    }

    private void OnFileDownLoadCompleted(bool ok, AssetVersion assetVersion)
    {
        if (ok)
        {
            okCount++;
            assetVersion.localValid = true;
        }
        else
        {
            var remoteURL = StringUtil.Contact(VersionUtility.Instance.versionInfo.GetResourcesURL(VersionConfig.Get().branch), "/", assetVersion.relativePath);
            var localURL = StringUtil.Contact(AssetPath.ExternalStorePath, assetVersion.relativePath);

            var remoteFile = new RemoteFile(remoteURL, localURL, assetVersion);
            CoroutineUtil.Instance.Coroutine(remoteFile.DownloadRemoteFile(OnFileDownLoadCompleted));
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
