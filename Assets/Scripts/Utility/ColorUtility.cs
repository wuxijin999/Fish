using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtility
{
    public readonly static Color white;
    public readonly static Color green;
    public readonly static Color blue;
    public readonly static Color purple;
    public readonly static Color orange;
    public readonly static Color red;
    public readonly static Color pink;

    static ColorUtility()
    {
        var colorPattern = Resources.Load<ColorPattern>("Config/ColorPattern");
        white = colorPattern.white;
        green = colorPattern.green;
        blue = colorPattern.blue;
        purple = colorPattern.purple;
        orange = colorPattern.orange;
        red = colorPattern.red;
        pink = colorPattern.pink;
    }

    public static Color SetR(this Color color, float r)
    {
        color.r = r;
        return color;
    }

    public static Color SetG(this Color color, float g)
    {
        color.g = g;
        return color;
    }

    public static Color SetB(this Color color, float b)
    {
        color.b = b;
        return color;
    }

    public static Color SetA(this Color color, float a)
    {
        color.a = a;
        return color;
    }



}

