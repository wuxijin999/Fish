using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public static class TimeUtil
{


    public static string ToTimeString(this int second, string pattern)
    {
        var timeSpan = new TimeSpan(second * TimeSpan.TicksPerSecond);

        //pattern = Regex.Replace(pattern, "HH", timeSpan.Hours.ToString());
        //pattern = Regex.Replace(pattern, "hh", (timeSpan.Hours % 12).ToString());
        //pattern = Regex.Replace(pattern, "mm", timeSpan.Minutes.ToString());
        //pattern = Regex.Replace(pattern, "ss", timeSpan.Seconds.ToString());
        // pattern = Regex.Replace(pattern, "dd", timeSpan.Days.ToString());

        return pattern;
    }




}
