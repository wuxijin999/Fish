using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class RichTextUtil
{
    const string pattern = "<[A-z]+=\\d+>";

    public static string Convert(string input, RichTextType type)
    {
        switch (type)
        {
            case RichTextType.Item:
                return Regex.Replace(input, pattern, ItemMatchEvaluator);
            case RichTextType.Npc:
                return Regex.Replace(input, pattern, NpcMatchEvaluator);
            default:
                break;
        }
        return "";
    }

    public static string Convert(string input)
    {
        return Regex.Replace(input, pattern, MatchEvaluator);
    }

    static string MatchEvaluator(Match match)
    {
        var type = GetRichTextType(match);
        switch (type)
        {
            case RichTextType.Item:
                return ItemMatchEvaluator(match);
            case RichTextType.Npc:
                return NpcMatchEvaluator(match);
            default:
                return string.Empty;
        }
    }

    static RichTextType GetRichTextType(Match match)
    {
        var titleMatch = Regex.Match(match.Value, "[A-z]+");
        if (titleMatch != null)
        {
            switch (titleMatch.Value)
            {
                case "Item":
                    return RichTextType.Item;
                case "Npc":
                    return RichTextType.Npc;
                default:
                    return RichTextType.None;
            }
        }
        else
        {
            return RichTextType.None;
        }
    }

    static string ItemMatchEvaluator(Match match)
    {
        var integerMatch = Regex.Match(match.Value, "\\d+");
        var itemId = integerMatch != null ? int.Parse(integerMatch.Value) : 0;
        var config = ItemConfig.Get(itemId);

        return config.name;
    }

    static string NpcMatchEvaluator(Match match)
    {
        var integerMatch = Regex.Match(match.Value, "\\d+");
        var id = integerMatch != null ? int.Parse(integerMatch.Value) : 0;
        var config = NpcConfig.Get(id);

        return config.name;
    }

    public enum RichTextType
    {
        None,
        Item,
        Npc,
    }

}
