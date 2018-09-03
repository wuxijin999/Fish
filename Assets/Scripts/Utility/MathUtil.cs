using UnityEngine;
using System;

public class MathUtil
{

    public static Vector3 Rotate90_XZ_CW(Vector3 vector)
    {
        Vector3 _vec = new Vector3(vector.z, vector.y, -vector.x);
        return _vec;
    }

    public static Vector3 Rotate90_XZ_CCW(Vector3 vector)
    {
        Vector3 _vec = new Vector3(-vector.z, vector.y, vector.x);
        return _vec;
    }

    public static Vector3 Rotate180_XZ(Vector3 vector)
    {
        Vector3 _vec = new Vector3(-vector.x, vector.y, -vector.z);
        return _vec;
    }

    public enum GestureType
    {
        Up,
        Down,
        Left,
        Right,
    }

    /// <summary>
    /// 返回手势方向
    /// </summary>
    /// <param name="_start"></param>
    /// <param name="_end"></param>
    /// <returns></returns>
    static public GestureType GetGestureDirection(Vector2 _start, Vector2 _end)
    {
        GestureType gesture;

        var direction = _end - _start;
        var x = direction.x;
        var y = direction.y;

        if (y < x && y > -x)
        {
            gesture = GestureType.Right;
        }
        else if (y > x && y < -x)
        {
            gesture = GestureType.Left;
        }
        else if (y > x && y > -x)
        {
            gesture = GestureType.Up;
        }
        else
        {
            gesture = GestureType.Down;
        }

        return gesture;
    }

    public static float FreeFall(float startY, float time)
    {

        float deltaY = 0.5f * GlobalDefine.GRAVITY_RATE * Mathf.Pow(time, 2f);

        return startY - deltaY;
    }

    public static float CalculateRefrenceScale(Vector2 designWH)
    {
        var width = designWH.x;
        var height = designWH.y;
        var refrenceHeight = 0f;

        if (Screen.height / (float)Screen.width > height / (float)width)
        {
            refrenceHeight = (float)width / Screen.width * Screen.height;
        }
        else
        {
            refrenceHeight = height;
        }

        var scale = refrenceHeight / height;

        return scale;
    }

    public static float CalDistance(Vector3 srcPos, Vector3 desPos)
    {
        return (srcPos.x - desPos.x) * (srcPos.x - desPos.x) + (srcPos.z - desPos.z) * (srcPos.z - desPos.z);
    }

    /// <summary>
    /// 返回Int数据中某一位是否为1
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
    /// <returns>true表示该位为1，false表示该位为0</returns>
    public static bool GetBitValue(uint value, ushort index)
    {
        if (index > 31)
        {
            throw new ArgumentOutOfRangeException("index"); //索引出错
        }

        var val = 1 << index;
        return (value & val) == val;
    }

    /// <summary>
    /// 设定Int数据中某一位的值
    /// </summary>
    /// <param name="value">位设定前的值</param>
    /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
    /// <param name="bitValue">true设该位为1,false设为0</param>
    /// <returns>返回位设定后的值</returns>
    public static int SetBitValue(int value, ushort index, bool bitValue)
    {
        if (index > 31)
        {
            throw new ArgumentOutOfRangeException("index"); //索引出错
        }

        var val = 1 << index;
        return bitValue ? (value | val) : (value & ~val);
    }

    public static bool IsPointInsideRectange(Vector3 point, Vector3 rectStart, Vector3 rectEnd)
    {
        return point.x > rectStart.x && point.x < rectEnd.x && point.z > rectStart.z && point.z < rectEnd.z;
    }

    /// <summary>
    /// 返回角度代表的四元素
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Quaternion GetClientRotationFromAngle(int angle)
    {
        float _angle = Mathf.Clamp(1.40625f * angle, 0f, 359f);
        return Quaternion.Euler(0, _angle, 0);
    }

    public static float DistanceSqrtXZ(Vector3 p1, Vector3 p2)
    {
        p1.y = 0;
        p2.y = 0;
        return Vector3.SqrMagnitude(p2 - p1);
    }

    public static Vector3 ForwardXZ(Vector3 target, Vector3 self)
    {
        target.y = 0;
        self.y = 0;
        return (target - self).normalized;
    }

    public static int Power(int a, int e)
    {
        int value = 1;
        for (int i = 0; i < e; i++)
        {
            value *= a;
        }

        return value;
    }

    public static bool CheckAdult(string _IDNumber)
    {
        if (string.IsNullOrEmpty(_IDNumber))
        {
            return false;
        }

        if (_IDNumber.Length == 15)
        {
            return true;
        }
        else if (_IDNumber.Length == 18)
        {
            var year = int.Parse(_IDNumber.Substring(6, 4));
            var month = int.Parse(_IDNumber.Substring(10, 2));
            var day = int.Parse(_IDNumber.Substring(12, 2));
            var borth = new DateTime(year, month, day);

            return (DateTime.Now - borth).TotalDays >= (365 * 18 + 4);
        }
        else
        {
            return true;
        }
    }

}
