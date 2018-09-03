using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForSecondConst
{
    public static readonly WaitForSeconds WaitMS100 = new WaitForSeconds(0.1f);
    public static readonly WaitForSeconds WaitMS200 = new WaitForSeconds(0.2f);
    public static readonly WaitForSeconds WaitMS300 = new WaitForSeconds(0.3f);
    public static readonly WaitForSeconds WaitMS400 = new WaitForSeconds(0.4f);
    public static readonly WaitForSeconds WaitMS500 = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds WaitMS600 = new WaitForSeconds(0.6f);
    public static readonly WaitForSeconds WaitMS700 = new WaitForSeconds(0.7f);
    public static readonly WaitForSeconds WaitMS800 = new WaitForSeconds(0.8f);
    public static readonly WaitForSeconds WaitMS900 = new WaitForSeconds(0.9f);
    public static readonly WaitForSeconds WaitMS1000 = new WaitForSeconds(1f);

    public static readonly WaitForSeconds WaitMS1100 = new WaitForSeconds(1.1f);
    public static readonly WaitForSeconds WaitMS1200 = new WaitForSeconds(1.2f);
    public static readonly WaitForSeconds WaitMS1300 = new WaitForSeconds(1.3f);
    public static readonly WaitForSeconds WaitMS1400 = new WaitForSeconds(1.4f);
    public static readonly WaitForSeconds WaitMS1500 = new WaitForSeconds(1.5f);
    public static readonly WaitForSeconds WaitMS1600 = new WaitForSeconds(1.6f);
    public static readonly WaitForSeconds WaitMS1700 = new WaitForSeconds(1.7f);
    public static readonly WaitForSeconds WaitMS1800 = new WaitForSeconds(1.8f);
    public static readonly WaitForSeconds WaitMS1900 = new WaitForSeconds(1.9f);
    public static readonly WaitForSeconds WaitMS2000 = new WaitForSeconds(2f);
    public static readonly WaitForSeconds WaitMS10000 = new WaitForSeconds(10f);

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
