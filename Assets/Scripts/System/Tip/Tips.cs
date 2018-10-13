//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, July 09, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : Presenter<Tips>, IPresenterWindow
{

    public readonly Dictionary<Type, Queue<string>> tipsQueue = new Dictionary<Type, Queue<string>>() {
        { Type.Normal,new Queue<string>()}
    };

    public readonly BoolProperty hasNewNormalTip = new BoolProperty();

    public void OpenWindow(object @object = null)
    {
    }

    public void CloseWindow()
    {
    }

    public void Show(Type type, string content)
    {
        if (!tipsQueue.ContainsKey(type))
        {
            tipsQueue[type] = new Queue<string>();
        }

        tipsQueue[type].Enqueue(content);
        Windows.Instance.Open(WindowType.Tip);
    }

    public void Show(string content)
    {
        tipsQueue[Type.Normal].Enqueue(content);
        hasNewNormalTip.value = true;
        Windows.Instance.Open(WindowType.Tip);
    }

    public enum Type
    {
        Normal,
    }

}





