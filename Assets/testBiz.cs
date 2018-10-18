using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBiz : MonoBehaviour
{

    WorldBossModel model = new WorldBossModel();
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CoroutineUtil.Instance.Begin(this.GetHashCode(), "Co_CountDown", Co_CountDown());
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CoroutineUtil.Instance.Stop(this.GetHashCode(), "Co_CountDown");
        }
    }

    IEnumerator Co_CountDown()
    {
        var timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 0f;
                Debug.LogFormat("现在时间是:{0},这个实例的hash是：{1}", Time.time, this.GetHashCode());
            }

            yield return null;
        }

    }


}

