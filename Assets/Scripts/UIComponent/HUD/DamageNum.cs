//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, October 18, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

public class DamageNum : UIBase
{
    [SerializeField] TextEx m_Text;
    [SerializeField] Tween[] m_Tweens;
    [SerializeField] FollowIgnoreY m_FollowTarget;

    Pattern m_Pattern = Pattern.Pattern1;
    public Pattern pattern {
        get { return m_Pattern; }
        private set { m_Pattern = value; }
    }

    float hideTime = 0f;

    static StringBuilder stringBuild = new StringBuilder();
    static int queueMax = 20;
    static Queue<DamageInfo> infoQueue = new Queue<DamageInfo>();

    public static void ShowDamage(DamageInfo info)
    {
        if (info.isPlayer)
        {
            while (infoQueue.Count >= queueMax)
            {
                infoQueue.Dequeue();
            }

            infoQueue.Enqueue(
                new DamageInfo()
                {
                    pattern = info.pattern,
                    num = info.num,
                    camera = info.camera,
                    target = info.target,
                    direction = info.direction,
                    isPlayer = info.isPlayer,
                }
                );
        }
        else
        {
            ShowDamageImmediately(info);
        }
    }

    public static void ShowDamageQueue()
    {
        if (infoQueue.Count > 0)
        {
            ShowDamageImmediately(infoQueue.Dequeue());
        }
    }

    static void ShowDamageImmediately(DamageInfo info)
    {
        var behaviour = DamageNumPool.Get(info.pattern);
        var uiPosition = CameraUtil.ConvertPosition(info.camera, UIRoot.uiCamera, info.target.position);

        behaviour.transform.SetParentEx(UIRoot.hudRoot.PickBloodCanvas().transform);
        behaviour.transform.position = uiPosition;
        behaviour.transform.localPosition = behaviour.transform.localPosition.SetZ(0);
        behaviour.transform.localScale = Vector3.one;
        behaviour.PopUp(info);
    }

    public void PopUp(DamageInfo info)
    {
        pattern = info.pattern;

        stringBuild.Remove(0, stringBuild.Length);
        var prefix = GetPrefiexKey(info.pattern);
        if (prefix > 0)
        {
            stringBuild.Append((char)prefix);
        }

        var symbol = GetSymbolKey(info.pattern);
        if (symbol > 0)
        {
            stringBuild.Append((char)symbol);
        }

        var numString = info.num.ToString();
        for (var i = 0; i < numString.Length; i++)
        {
            stringBuild.Append(GetNumKey(pattern, numString[i]));
        }

        m_Text.text = stringBuild.ToString();
        this.gameObject.SetActive(true);

        hideTime = Time.time;
        foreach (var item in m_Tweens)
        {
            item.Play(true);
            if (Time.time + item.duration > hideTime)
            {
                hideTime = Time.time + item.duration;
            }
        }
    }

    int GetPrefiexKey(Pattern pattern)
    {
        var config = DamageNumConfig.Get((int)pattern);
        return config.prefix;
    }

    int GetSymbolKey(Pattern pattern)
    {
        var config = DamageNumConfig.Get((int)pattern);
        return config.symbol;
    }

    int GetNumKey(Pattern pattern, int num)
    {
        var config = DamageNumConfig.Get((int)this.pattern);
        return config.nums[num - 48];
    }

    void LateUpdate()
    {
        if (Time.time > hideTime)
        {
            DamageNumPool.Release(this);
        }
    }

    public enum Pattern
    {
        Pattern1 = 1,
        Pattern2 = 2,
    }

    public struct DamageInfo
    {
        public Pattern pattern;
        public int num;
        public Camera camera;
        public Transform target;
        public Vector3 direction;
        public bool isPlayer;
    }



}






