//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPresenter : Presenter<LoadingPresenter>, IPresenterInit, IPresenterUnInit
{
    public readonly IntProperty sceneId = new IntProperty();
    public readonly FloatProperty progress = new FloatProperty();

    public void Init()
    {
        SceneLoad.Instance.progressChangeEvent += OnProgressChange;
    }

    public void UnInit()
    {
        SceneLoad.Instance.progressChangeEvent -= OnProgressChange;
    }

    public override void OpenWindow(int functionId = 0)
    {
    }

    public override void CloseWindow()
    {
    }

    public void OpenWindow(int sceneId, int functionId = 0)
    {
        this.sceneId.value = sceneId;
    }

    private void OnProgressChange(float progress)
    {
        this.progress.value = progress;
    }


}





