using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.iOS;

public class DeviceUtil
{

    static string mac = string.Empty;
    public static string GetMac()
    {
        if (string.IsNullOrEmpty(mac))
        {
            try
            {
                var netInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                if (netInterfaces != null && netInterfaces.Length > 0)
                {
                    mac = netInterfaces[0].GetPhysicalAddress().ToString();
                }
            }
            catch (Exception ex)
            {
                DebugEx.Log(ex);
            }
        }

        return mac;
    }

    static string ipPattern = "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}";
    static string ip = string.Empty;
    public static string GetIp()
    {
        if (string.IsNullOrEmpty(ip))
        {
            var url = "http://pv.sohu.com/cityjson";
            var wRequest = WebRequest.Create(url);
            wRequest.Method = "GET";
            wRequest.ContentType = "text/html;charset=UTF-8";
            var wResponse = wRequest.GetResponse();
            var stream = wResponse.GetResponseStream();
            var reader = new StreamReader(stream, System.Text.Encoding.Default);
            var str = reader.ReadToEnd();

            reader.Close();
            wResponse.Close();

            var match = Regex.Match(str, ipPattern);
            if (match != null)
            {
                ip = match.Value;
            }
        }

        return ip;
    }

    public static string GetDeviceUniquenessIdentify()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                return Device.advertisingIdentifier;
            default:
                return SystemInfo.deviceUniqueIdentifier;
        }
    }

    public static string GetDeviceOSLevel()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                return Device.systemVersion;
            default:
                return SystemInfo.operatingSystem;
        }
    }

    public static string GetDeviceName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                return Device.generation.ToString();
            default:
                return SystemInfo.deviceName;
        }
    }

    public static string GetDeviceModel()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                return Device.generation.ToString();
            default:
                return SystemInfo.deviceModel;
        }
    }

    public static bool IsLowMemory()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            switch (Device.generation)
            {
                case DeviceGeneration.iPadMini2Gen:
                    return true;
                case DeviceGeneration.iPhone5S:
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
#endif
        return false;
    }

}
