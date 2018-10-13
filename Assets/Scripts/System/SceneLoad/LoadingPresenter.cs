//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPresenter : Presenter<LoadingPresenter>, IPresenterInit, IPresenterUnInit, IPresenterWindow
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

    public void OpenWindow(object @object)
    {
    }

    public void OpenWindow(int sceneId, int functionId)
    {
        this.sceneId.value = sceneId;
    }

    public void CloseWindow()
    {
    }

    private void OnProgressChange(float progress)
    {
        this.progress.value = progress;
    }


}





