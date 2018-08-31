using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtil
{
    public readonly static Color white;
    public readonly static Color green;
    public readonly static Color blue;
    public readonly static Color purple;
    public readonly static Color orange;
    public readonly static Color red;
    public readonly static Color pink;

    readonly static string[] qualityColors;

    static ColorUtil()
    {
        var colorPattern = Resources.Load<ColorPattern>("Config/ColorPattern");
        white = colorPattern.white;
        green = colorPattern.green;
        blue = colorPattern.blue;
        purple = colorPattern.purple;
        orange = colorPattern.orange;
        red = colorPattern.red;
        pink = colorPattern.pink;

        qualityColors = new string[] {
         StringUtil.Contact("<color=#", ColorToInt16String(white), ">{0}</color>"),
         StringUtil.Contact("<color=#", ColorToInt16String(green), ">{0}</color>"),
         StringUtil.Contact("<color=#", ColorToInt16String(blue), ">{0}</color>"),
         StringUtil.Contact("<color=#", ColorToInt16String(purple), ">{0}</color>"),
         StringUtil.Contact("<color=#", ColorToInt16String(orange), ">{0}</color>"),
         StringUtil.Contact("<color=#", ColorToInt16String(red), ">{0}</color>"),
         StringUtil.Contact("<color=#", ColorToInt16String(pink), ">{0}</color>"),
        };
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


    public static Color QualityColor(int _quality)
    {
        var quality = Mathf.Clamp(_quality - 1, 0, 6);
        switch (quality)
        {
            case 0:
                return white;
            case 1:
                return green;
            case 2:
                return blue;
            case 3:
                return purple;
            case 4:
                return orange;
            case 5:
                return red;
            case 6:
                return pink;
            default:
                return white;
        }
    }

    public static string QualityColorString(int _quality, string _input)
    {
        var quality = Mathf.Clamp(_quality - 1, 0, 6);
        return string.Format(qualityColors[quality], _input);
    }

    static string ColorToInt16String(Color _color)
    {
        var rInt = Mathf.RoundToInt(_color.r * 255);
        var r1 = System.Convert.ToString(rInt / 16, 16);
        var r2 = System.Convert.ToString(rInt % 16, 16);

        var gInt = Mathf.RoundToInt(_color.g * 255);
        var g1 = System.Convert.ToString(gInt / 16, 16);
        var g2 = System.Convert.ToString(gInt % 16, 16);

        var bInt = Mathf.RoundToInt(_color.b * 255);
        var b1 = System.Convert.ToString(bInt / 16, 16);
        var b2 = System.Convert.ToString(bInt % 16, 16);

        var aInt = Mathf.RoundToInt(_color.a * 255);
        var a1 = System.Convert.ToString(aInt / 16, 16);
        var a2 = System.Convert.ToString(aInt % 16, 16);

        return StringUtil.Contact(r1, r2, g1, g2, b1, b2, a1, a2);
    }
}

