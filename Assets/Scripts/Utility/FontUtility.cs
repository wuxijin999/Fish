using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontUtility
{
    static Font m_Preferred;
    public static Font preferred {
        get { return m_Preferred ?? (m_Preferred = Resources.Load<Font>("Font/方正准圆简体")); }
    }

    static Font m_Secondary;
    public static Font secondary {
        get { return m_Secondary ?? (m_Secondary = Resources.Load<Font>("Font/方正隶变简体")); }
    }

}
