using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionUtil
{
    public static readonly Vector2 originalResolution = new Vector2(Screen.width, Screen.height);
    public static Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

    public static void AdjustResolution()
    {
        Screen.SetResolution(Mathf.RoundToInt(currentResolution.x), Mathf.RoundToInt(currentResolution.y), true);
    }

    public static void AdjustResolution(GameQuality quality)
    {
        switch (quality)
        {
            case GameQuality.Low:
                currentResolution = ConvertResolution(new Vector2(960, 540));
                break;
            case GameQuality.Medium:
                currentResolution = ConvertResolution(new Vector2(1280, 720));
                break;
            case GameQuality.High:
                currentResolution = ConvertResolution(new Vector2(1920, 1080));
                break;
        }

        Screen.SetResolution(Mathf.RoundToInt(currentResolution.x), Mathf.RoundToInt(currentResolution.y), true);
    }

    static Vector2 ConvertResolution(Vector2 _inputResolution)
    {
        var resolution = Screen.currentResolution;
        var ratio = (resolution.width / (float)resolution.height) / ((float)16 / 9);

        var height = 0f;
        var width = 0f;
        if (ratio > 1)
        {
            height = _inputResolution[1];
            width = Mathf.RoundToInt(resolution.width / (float)resolution.height * height);
        }
        else
        {
            width = _inputResolution[0];
            height = Mathf.RoundToInt((float)resolution.height / resolution.width * width);
        }

        if (height * width - originalResolution.x * originalResolution.y > 10)
        {
            return originalResolution;
        }
        else
        {
            return new Vector2(width, height);
        }

    }

}
