//----------------------------------------------------------------------
//   这是一个全局类型，作为Fish的狗腿子，整个项目中具有最高地位
//   没有得到主程许可 , 不允许更改这个类型
//----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAgent : SingletonMonobehaviour<FishAgent>
{

    void OnApplicationQuit()
    {
        Fish.Broadcast(BroadcastType.ApplicationOut);
    }

    void OnApplicationPause(bool pause)
    {
        Fish.Broadcast(pause ? BroadcastType.ApplicationPause : BroadcastType.ApplicationUnPause);
    }

    void OnApplicationFocus(bool focus)
    {
        Fish.Broadcast(focus ? BroadcastType.ApplicationFocus : BroadcastType.ApplicationUnFocus);
    }

}
