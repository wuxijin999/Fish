using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
using System;

public class AssetsCopyTask
{
    public static string copiedVersion {
        get { return LocalSave.GetString("AssestsCopyVersion"); }
        set { LocalSave.SetString("AssestsCopyVersion", value); }
    }

    object progressLock = new object();
    float m_Progress = 0f;
    public float progress {
        get {
            return m_Progress;
        }
        set {
            lock (progressLock)
            {
                m_Progress = value;
            }
        }
    }

    object doneLock = new object();
    bool m_Done = false;
    public bool done {
        get {
            return m_Done;
        }
        set {
            lock (doneLock)
            {
                m_Done = value;
            }
        }
    }

    string versionBuf;

    public static AssetsCopyTask BeginCopy(string version)
    {
        var task = new AssetsCopyTask(version);
        return task;
    }

    public AssetsCopyTask(string version)
    {
        this.versionBuf = version;
    }

    public void Begin()
    {
        done = false;
        var fromRoot = AssetPath.StreamingAssetPath;
        var toRoot = AssetPath.ExternalStorePath;

        ThreadPool.QueueUserWorkItem((object x) =>
        {
            var allFiles = new List<FileInfo>();
            FileExtension.GetAllDirectoryFileInfos(fromRoot, ref allFiles);
            var count = allFiles.Count;
            var index = 0;
            foreach (var item in allFiles)
            {
                try
                {
                    var fromFile = item.FullName;
                    var toFile = fromFile.Replace(fromRoot, toRoot);
                    if (File.Exists(toFile))
                    {
                        continue;
                    }

                    var toDirectory = Path.GetDirectoryName(toFile);
                    if (!Directory.Exists(toDirectory))
                    {
                        Directory.CreateDirectory(toDirectory);
                    }

                    DebugEx.LogFormat("拷贝文件：{0}", item.Name);
                    File.Copy(fromFile, toFile, true);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
                finally
                {
                    progress = (float)++index / count;
                }
            }

            done = true;
            copiedVersion = versionBuf;
        });

    }

}

