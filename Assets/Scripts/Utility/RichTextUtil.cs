using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class RichTextUtil
{


    const string itemPattern = "<Item=[0,9]+>";



    public static string Convert(string text)
    {

        return "";
    }

    public static string Convert(string text, RichTextType type)
    {
        switch (type)
        {
            case RichTextType.Item:
                var matches = Regex.Matches(text, itemPattern);
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    var integerMatch = Regex.Match(match.Value, "\\d+");
                    var itemId = integerMatch != null ? int.Parse(integerMatch.Value) : 0;
                    var config = ItemConfig.Get(itemId);

                }
                break;
            default:
                break;
        }
        return "";
    }


    public enum RichTextType
    {
        Item,
    }

}
