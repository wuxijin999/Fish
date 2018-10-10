using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class Launch : MonoBehaviour
{

    private void Awake()
    {
    }

    void Start()
    {
        StartCoroutine(Co_Launch());
    }

    IEnumerator Co_Launch()
    {
        ConfigInitiator.PreInit();

        if (AssetsCopyTask.copiedVersion != VersionConfig.Get().version)
        {
            var assetCopy = AssetsCopyTask.BeginCopy(VersionConfig.Get().version);
            while (!assetCopy.isDone)
            {
                yield return null;
            }
        }

        ConfigInitiator.Init();
        SceneLoad.Instance.LoadLogin();
    }

    public class AssetsCopyTask
    {
        public static string copiedVersion {
            get { return LocalSave.GetString("AssestsCopyVersion"); }
            set { LocalSave.SetString("AssestsCopyVersion", value); }
        }

        public bool isDone { get; private set; }
        public float progress { get; private set; }

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
            isDone = false;
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

                isDone = true;
                copiedVersion = versionBuf;
            });

        }

    }

}
