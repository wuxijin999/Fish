using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForConst
{
    public static readonly WaitForSeconds millisecond100 = new WaitForSeconds(0.1f);
    public static readonly WaitForSeconds millisecond200 = new WaitForSeconds(0.2f);
    public static readonly WaitForSeconds millisecond300 = new WaitForSeconds(0.3f);
    public static readonly WaitForSeconds millisecond400 = new WaitForSeconds(0.4f);
    public static readonly WaitForSeconds millisecond500 = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds millisecond600 = new WaitForSeconds(0.6f);
    public static readonly WaitForSeconds millisecond700 = new WaitForSeconds(0.7f);
    public static readonly WaitForSeconds millisecond800 = new WaitForSeconds(0.8f);
    public static readonly WaitForSeconds millisecond900 = new WaitForSeconds(0.9f);
    public static readonly WaitForSeconds millisecond1000 = new WaitForSeconds(1f);

    public static readonly WaitForSeconds millisecond1100 = new WaitForSeconds(1.1f);
    public static readonly WaitForSeconds millisecond1200 = new WaitForSeconds(1.2f);
    public static readonly WaitForSeconds millisecond1300 = new WaitForSeconds(1.3f);
    public static readonly WaitForSeconds millisecond1400 = new WaitForSeconds(1.4f);
    public static readonly WaitForSeconds millisecond1500 = new WaitForSeconds(1.5f);
    public static readonly WaitForSeconds millisecond1600 = new WaitForSeconds(1.6f);
    public static readonly WaitForSeconds millisecond1700 = new WaitForSeconds(1.7f);
    public static readonly WaitForSeconds millisecond1800 = new WaitForSeconds(1.8f);
    public static readonly WaitForSeconds millisecond1900 = new WaitForSeconds(1.9f);
    public static readonly WaitForSeconds millisecond2000 = new WaitForSeconds(2f);
    public static readonly WaitForSeconds millisecond10000 = new WaitForSeconds(10f);

    private static Dictionary<float, WaitForSeconds> m_WaitDict = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWaitForSeconds(float t)
    {
        if (m_WaitDict.ContainsKey(t))
        {
            return m_WaitDict[t];
        }
        else
        {
            var _wait = new WaitForSeconds(t);
            m_WaitDict.Add(t, _wait);
            return _wait;
        }
    }

}
