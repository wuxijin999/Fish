using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Int2
{
    public int x;
    public int y;

    public Int2(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    public int this[int index] {
        get {
            return index == 0 ? x : index == 1 ? y : 0;
        }
        set {
            if (index == 0)
            {
                this.x = value;
            }
            else if (index == 1)
            {
                this.y = value;
            }
        }
    }

    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }
        if ((other.GetType().Equals(this.GetType())) == false)
        {
            return false;
        }

        var temp = (Int2)other;
        return this.x.Equals(temp.x) && this.y.Equals(temp.y);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() + this.y.GetHashCode();
    }

    public static bool operator ==(Int2 lhs, Int2 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Int2 lhs, Int2 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y;
    }

    public static Int2 operator +(Int2 a, Int2 b)
    {
        return new Int2(a.x + b.x, a.y + b.y);
    }

    public static Int2 operator -(Int2 a, Int2 b)
    {
        return new Int2(a.x - b.x, a.y - b.y);
    }

}

public struct Int3
{
    public int x;
    public int y;
    public int z;

    public Int3(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
        this.z = 0;
    }

    public Int3(int _x, int _y, int _z)
    {
        this.x = _x;
        this.y = _y;
        this.z = _z;
    }

    public int this[int index] {
        get {
            return index == 0 ? this.x : index == 1 ? this.y : index == 2 ? this.z : 0;
        }
        set {
            if (index == 0)
            {
                this.x = value;
            }
            else if (index == 1)
            {
                this.y = value;
            }
            else if (index == 2)
            {
                this.z = value;
            }
        }
    }

    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }
        if ((other.GetType().Equals(this.GetType())) == false)
        {
            return false;
        }

        var temp = (Int3)other;
        return this.x.Equals(temp.x) && this.y.Equals(temp.y) && this.z.Equals(temp.z);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() + this.y.GetHashCode() + this.z.GetHashCode();
    }

    public static bool operator ==(Int3 lhs, Int3 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(Int3 lhs, Int3 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
    }

    public static Int3 operator +(Int3 a, Int3 b)
    {
        return new Int3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Int3 operator -(Int3 a, Int3 b)
    {
        return new Int3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

}

