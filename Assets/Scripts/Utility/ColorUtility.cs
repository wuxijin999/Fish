using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtility {

    public static readonly Color dwhite = new Color(1f,1f,1f,1f);
    public static readonly Color dgray = new Color(0.5f,0.5f,0.5f,1f);

    public static Color SetR(this Color color,float r) {
        color.r = r;
        return color;
    }

    public static Color SetG(this Color color,float g) {
        color.g = g;
        return color;
    }

    public static Color SetB(this Color color,float b) {
        color.b = b;
        return color;
    }

    public static Color SetA(this Color color,float a) {
        color.a = a;
        return color;
    }

}

