using UnityEngine;
using System.Text.RegularExpressions;

public static class VectorUtil
{

    public static Vector3 SetX(this Vector3 vector3, float x)
    {
        vector3.x = x;
        return vector3;
    }

    public static Vector3 SetY(this Vector3 vector3, float y)
    {
        vector3.y = y;
        return vector3;
    }

    public static Vector3 SetZ(this Vector3 vector3, float z)
    {
        vector3.z = z;
        return vector3;
    }

    public static Vector2 SetX(this Vector2 vector2, float x)
    {
        vector2.x = x;
        return vector2;
    }

    public static Vector2 SetY(this Vector2 vector2, float y)
    {
        vector2.y = y;
        return vector2;
    }

    public static Vector3 Vector3Parse(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Vector3.zero;
        }

        input = input.Replace("(", "").Replace(")", "");
        var stringArray = input.Split(',');

        var x = 0f;
        var y = 0f;
        var z = 0f;
        if (stringArray.Length > 0)
        {
            float.TryParse(stringArray[0], out x);
        }

        if (stringArray.Length > 1)
        {
            float.TryParse(stringArray[1], out y);
        }

        if (stringArray.Length > 2)
        {
            float.TryParse(stringArray[2], out z);
        }

        return new Vector3(x, y, z);
    }

}
