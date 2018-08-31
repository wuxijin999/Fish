using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
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
            string url = "http://pv.sohu.com/cityjson";
            WebRequest wRequest = WebRequest.Create(url);
            wRequest.Method = "GET";
            wRequest.ContentType = "text/html;charset=UTF-8";
            WebResponse wResponse = wRequest.GetResponse();
            Stream stream = wResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default);
            string str = reader.ReadToEnd();

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
#if UNITY_IOS
        return UnityEngine.iOS.Device.advertisingIdentifier;
# elif UNITY_ANDROID
        return SystemInfo.deviceUniqueIdentifier;
#else
        return "";
#endif
    }

    public static string GetDeviceOSLevel()
    {
#if UNITY_IOS
        return UnityEngine.iOS.Device.systemVersion;
# elif UNITY_ANDROID
        return SystemInfo.operatingSystem;
#else
        return "";
#endif
    }

    public static string GetDeviceName()
    {
#if UNITY_IOS
        return UnityEngine.iOS.Device.generation.ToString();
#elif UNITY_ANDROID
        return SystemInfo.deviceName;
#else
        return "";
#endif
    }

    public static string GetDeviceModel()
    {
#if UNITY_IOS
        return UnityEngine.iOS.Device.generation.ToString();
#elif UNITY_ANDROID
        return SystemInfo.deviceModel;
#else
        return "";
#endif
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
