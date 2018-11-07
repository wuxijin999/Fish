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

public class VersionUtil : Singleton<VersionUtil>
{
    public const string VERSION_URL = "http://pub.game.secondworld.net.cn:11000/appversion/?";

    public string androidRoot { get; private set; }

    public float progress {
        get { return 0 / ((float)this.versionInfo.GetLatestVersion().file_size * 1024); }
    }

    public string apkLocalURL = string.Empty;
    public VersionInfo versionInfo { get; private set; }
    public bool completed { get { return this.step == Step.Completed; } }

    Step m_Step = Step.None;
    public Step step {
        get { return this.m_Step; }
        private set {
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

        var url = StringUtil.Contact(VERSION_URL, HttpRequest.HashTableToString(tables));
        HttpRequest.Instance.RequestHttpGet(url, this.OnVersionCheckResult);
    }

    private void OnVersionCheckResult(bool ok, string result)
    {
        if (ok)
        {
            this.versionInfo = JsonMapper.ToObject<VersionInfo>(result);
            if (this.versionInfo.VersionCount > 0)
            {
                var version = this.versionInfo.GetLatestVersion();
                var remoteURL = version.download_url;

                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        var fileName = Path.GetFileName(remoteURL);
                        this.apkLocalURL = StringUtil.Contact(this.androidRoot, "/", fileName);
                        if (File.Exists(this.apkLocalURL))
                        {
                            this.step = Step.ApkExist;
                        }
                        else
                        {
                            this.step = Step.DownLoadPrepared;
                        }
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        this.step = Step.DownLoadPrepared;
                        break;
                    default:
                        this.step = Step.Completed;
                        break;
                }
            }
            else
            {
                this.step = Step.Completed;

                var apkFiles = new DirectoryInfo(AssetPath.ExternalStorePath).GetFiles("*.apk");
                for (int i = apkFiles.Length - 1; i >= 0; i--)
                {
                    File.Delete(apkFiles[i].FullName);
                }
            }
        }
        else
        {
            this.step = Step.None;
            var clockSetting = new Clock.ClockParams()
            {
                type = Clock.ClockType.UnityRealTimeClock,
                second = 1,
            };

            ClockUtil.Instance.Create(clockSetting, this.RequestVersionCheck);
        }
    }

    public void StartDownLoad()
    {
        this.step = Step.DownLoad;
        var version = this.versionInfo.GetLatestVersion();
        var remoteURL = version.download_url;
        var fileName = Path.GetFileName(remoteURL);
        this.apkLocalURL = StringUtil.Contact(this.androidRoot, "/", fileName);
    }

    private void OnDownLoadApkCompleted(bool ok, AssetVersion assetVersion)
    {
        if (ok)
        {
            this.step = Step.Completed;
        }
        else
        {
            this.step = Step.DownLoadFailed;
        }
    }

    public void SkipVersion()
    {
        this.step = Step.Completed;
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
            if (this.versions.Length > 0)
            {
                return this.versions[0];
            }
            else
            {
                return default(Version);
            }
        }

        public string GetResourcesURL(int _branch)
        {
            if (this.resource_url != null)
            {
                return this.resource_url[_branch.ToString()].ToString();
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
                if (this.notice_flag != null)
                {
                    return this.notice_flag[_branch.ToString()].ToString();
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




