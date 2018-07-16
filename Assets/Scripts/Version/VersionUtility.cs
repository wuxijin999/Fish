//--------------------------------------------------------
//    [Author]:           第二世界
//    [  Date ]:           Thursday, March 15, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using LitJson;
using System.IO;

public class VersionUtility : Singleton<VersionUtility>
{
    public const string VERSION_URL = "http://pub.game.secondworld.net.cn:11000/appversion/?";
    const string VERSION_URL_PURE = "http://pub.game.secondworld.net.cn:11000/purge/appversion/?";

    public string androidRoot { get { return VersionConfig.Get().bundleIdentifier; } }

    public float progress {
        get { return RemoteFile.TotalDownloadSize / ((float)versionInfo.GetLatestVersion().file_size * 1024); }
    }

    public string apkLocalURL = string.Empty;
    public VersionInfo versionInfo { get; private set; }
    public bool completed { get { return step == Step.Completed; } }

    Step m_Step = Step.None;
    public Step step {
        get { return m_Step; }
        private set {
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
    public event Action<Step> downLoadStepChangeEvent;

    public void RequestVersionCheck()
    {
        var tables = new Dictionary<string, string>();
        tables["channel"] = VersionConfig.Get().appId;
        tables["versioncode"] = VersionConfig.Get().version;
        if (VersionConfig.Get().branch != 0)
        {
            tables["branch"] = VersionConfig.Get().branch.ToString();
        }

        var url = StringUtility.Contact(VERSION_URL, HttpRequest.HashTableToString(tables));
        HttpRequest.Instance.RequestHttpGet(url, HttpRequest.defaultHttpContentType, 1, OnVersionCheckResult);
    }

    private void OnVersionCheckResult(bool _ok, string _result)
    {
        if (_ok)
        {
            versionInfo = JsonMapper.ToObject<VersionInfo>(_result);
            if (versionInfo.VersionCount > 0)
            {
                var version = versionInfo.GetLatestVersion();
                var remoteURL = version.download_url;

                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        var fileName = Path.GetFileName(remoteURL);
                        apkLocalURL = StringUtility.Contact(androidRoot, "/", fileName);
                        if (File.Exists(apkLocalURL))
                        {
                            step = Step.ApkExist;
                        }
                        else
                        {
                            step = Step.DownLoadPrepared;
                        }
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        step = Step.DownLoadPrepared;
                        break;
                    default:
                        step = Step.Completed;
                        break;
                }
            }
            else
            {
                step = Step.Completed;

                var apkFiles = new DirectoryInfo(AssetPath.ExternalStorePath).GetFiles("*.apk");
                for (int i = apkFiles.Length - 1; i >= 0; i--)
                {
                    File.Delete(apkFiles[i].FullName);
                }
            }
        }
        else
        {
            step = Step.None;
            Clock.Create(DateTime.Now + new TimeSpan(TimeSpan.TicksPerSecond), RequestVersionCheck);
        }
    }

    public void StartDownLoad()
    {
        step = Step.DownLoad;
        var version = versionInfo.GetLatestVersion();
        var remoteURL = version.download_url;
        var fileName = Path.GetFileName(remoteURL);
        apkLocalURL = StringUtility.Contact(androidRoot, "/", fileName);
        var remoteFile = new RemoteFile(remoteURL, apkLocalURL, null);
        RemoteFile.Prepare();
        CoroutineUtility.Instance.Coroutine(remoteFile.DownloadRemoteFile(OnDownLoadApkCompleted));
    }

    private void OnDownLoadApkCompleted(bool _ok, AssetVersion _assetVersion)
    {
        if (_ok)
        {
            step = Step.Completed;
        }
        else
        {
            step = Step.DownLoadFailed;
        }
    }

    public void SkipVersion()
    {
        step = Step.Completed;
    }

    public class VersionInfo
    {
        public int ForceCount;
        public int VersionCount;
        public JsonData resource_url;
        public JsonData notice_flag;
        public Version[] versions;

        public Version GetLatestVersion()
        {
            if (versions.Length > 0)
            {
                return versions[0];
            }
            else
            {
                return default(Version);
            }
        }

        public string GetResourcesURL(int _branch)
        {
            if (resource_url != null)
            {
                return resource_url[_branch.ToString()].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetNoticeURL(int _branch)
        {
            try
            {
                if (notice_flag != null)
                {
                    return notice_flag[_branch.ToString()].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                DebugEx.Log(ex);
                return string.Empty;
            }
        }

    }

    public struct Version
    {
        public int is_fullpackage;
        public int file_size;
        public string download_url;
        public string version_desc;
        public DateTime public_time;
        public int update_force;
        public string version_name;
    }

    public enum Step
    {
        None,
        ApkExist,
        DownLoadPrepared,
        DownLoad,
        DownLoadFailed,
        Completed,
    }

}




