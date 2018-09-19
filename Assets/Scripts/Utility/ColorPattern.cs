using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/ColorPattern")]
public class ColorPattern : ScriptableObject
{
    [SerializeField] Color m_White;
    public Color white { get { return this.m_White; } }

    [SerializeField] Color m_Green;
    public Color green { get { return this.m_Green; } }

    [SerializeField] Color m_Blue;
    public Color blue { get { return this.m_Blue; } }

    [SerializeField] Color m_Purple;
    public Color purple { get { return this.m_Purple; } }

    [SerializeField] Color m_Orange;
    public Color orange { get { return this.m_Orange; } }

    [SerializeField] Color m_Red;
    public Color red { get { return this.m_Red; } }

    [SerializeField] Color m_Pink;
    public Color pink { get { return this.m_Pink; } }

    [SerializeField] Color m_Gray;
    public Color gray { get { return this.m_Gray; } }


}
