﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public struct Int2
{
    public int x;
    public int y;

    public Int2(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    public int this[int index]
    {
        get
        {
            return index == 0 ? this.x : index == 1 ? this.y : 0;
        }
        set
        {
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

    public static Int2 zero = new Int2(0, 0);

    public static bool TryParse(string input, out Int2 _value)
    {
        if (string.IsNullOrEmpty(input))
        {
            _value = Int2.zero;
            return false;
        }
        else
        {
            var matches = Regex.Matches(input.Trim(), "[-]{0,1}\\d+");
            if (matches.Count == 2)
            {
                _value = new Int2(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
                return true;
            }
            else
            {
                _value = Int2.zero;
                return false;
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

    public override string ToString()
    {
        return string.Format("({0},{1})", this.x, this.y);
    }

    public static bool operator ==(Int2 lhs, Int2 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Int2 lhs, Int2 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y;
    }

    public static Int2 operator +(Int2 lhs, Int2 rhs)
    {
        return new Int2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    public static Int2 operator -(Int2 lhs, Int2 rhs)
    {
        return new Int2(lhs.x - rhs.x, lhs.y - rhs.y);
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

    public int this[int index]
    {
        get
        {
            return index == 0 ? this.x : index == 1 ? this.y : index == 2 ? this.z : 0;
        }
        set
        {
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

    public static Int3 zero = new Int3(0, 0, 0);

    public static bool TryParse(string input, out Int3 _value)
    {
        if (string.IsNullOrEmpty(input))
        {
            _value = Int3.zero;
            return false;
        }
        else
        {
            var matches = Regex.Matches(input.Trim(), "[-]{0,1}\\d+");
            if (matches.Count == 2)
            {
                _value = new Int3(int.Parse(matches[0].Value), int.Parse(matches[1].Value), 0);
                return true;
            }
            else if (matches.Count == 3)
            {
                _value = new Int3(int.Parse(matches[0].Value), int.Parse(matches[1].Value), int.Parse(matches[2].Value));
                return true;
            }
            else
            {
                _value = Int3.zero;
                return false;
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

    public override string ToString()
    {
        return string.Format("({0},{1},{2})", this.x, this.y, this.z);
    }

    public static bool operator ==(Int3 lhs, Int3 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(Int3 lhs, Int3 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
    }

    public static Int3 operator +(Int3 lhs, Int3 rhs)
    {
        return new Int3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    }

    public static Int3 operator -(Int3 lhs, Int3 rhs)
    {
        return new Int3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }

}

