using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class FishSocket
{

    Thread recieveThread;
    byte[] bufferBytes = new byte[4096];                       // 4K，单包字节数组缓存
    byte[] fragmentBytes;                                               //留包后的内容

    string ip;
    int port;

    object socketLockObject = new object();
    Socket m_Socket;
    Socket socket {
        get {
            lock (socketLockObject)
            {
                return m_Socket;
            }
        }
        set {
            lock (socketLockObject)
            {
                m_Socket = value;
            }
        }
    }

    public bool connected { get { return socket == null ? false : socket.Connected; } }
    bool working = false;
    float lastTimeGotNetPackage = Time.realtimeSinceStartup;
    Action<bool> onConnected = null;

    public FishSocket()
    {
    }

    public void Connect(string ip, int port, Action<bool> onConnect)
    {
        try
        {
            this.ip = ip;
            this.port = port;
            this.onConnected = onConnect;
            Dns.BeginGetHostAddresses(ip, OnGetHostAddresses, null);
        }
        catch (Exception e)
        {
            DebugEx.LogError(e.Message);
        }
    }

    private void OnGetHostAddresses(IAsyncResult result)
    {
        var ipAddresses = Dns.EndGetHostAddresses(result);
        if (ipAddresses == null || ipAddresses.Length == 0)
        {
            Debug.Log("invalid host addresses .");
            return;
        }

        var endPoint = new IPEndPoint(ipAddresses[0], port);
        if (endPoint == null)
        {
            Debug.Log("endPoint is null .");
            return;
        }

        var isIPV6 = ipAddresses[0].AddressFamily == AddressFamily.InterNetworkV6;
        socket = new Socket(isIPV6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect(endPoint, new AsyncCallback(ConnectCallBack), null);
    }

    private void ConnectCallBack(IAsyncResult result)
    {
        try
        {
            if (result.IsCompleted)
            {
                if (recieveThread != null)
                {
                    recieveThread.Abort();
                }

                working = true;
                recieveThread = new Thread(new ThreadStart(OnReceiveInfo));
                recieveThread.IsBackground = true;
                recieveThread.Start();
            }
            else
            {
                CloseConnect();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            if (onConnected != null)
            {
                onConnected(result.IsCompleted);
                onConnected = null;
            }
        }
    }

    /// <summary>
    /// 关闭链接
    /// </summary>
    public void CloseConnect()
    {
        working = false;
        try
        {
            if (connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
        finally
        {
            sendQueue.Clear();
            socket = null;
        }
    }

    private void OnReceiveInfo()
    {
        while (working)
        {
            if (!connected)
            {
                CloseConnect();
                break;
            }

            try
            {
                var dataLength = socket.Receive(bufferBytes);
                if (dataLength <= 0)
                {
                    CloseConnect();
                    break;
                }

                var bytes = new byte[dataLength];
                Array.Copy(bufferBytes, 0, bytes, 0, dataLength);
                ReadInfo(bytes);
            }
            catch (Exception e)
            {
                DebugEx.Log(e);
            }
        }
    }

    private void ReadInfo(byte[] bytes)
    {
        try
        {
        }
        catch (Exception ex)
        {
            Debug.LogFormat("收包异常：{0}", ex);
        }
    }

    public void SendInfo(byte[] bytes)
    {
        if (!working)
        {
            return;
        }

        if (!connected)
        {
            DebugEx.LogError("尚未与该后端链接！无法发送信息");
            return;
        }

        if (bytes == null || bytes.Length < 2)
        {
            DebugEx.LogError("要发的信息数据为空或数据不足");
            return;
        }

        bytes = NetEnCoder.EnCode(bytes);
        SendBytes(bytes);
    }

    Queue<byte[]> sendQueue = new Queue<byte[]>();

    private void SendBytes(byte[] bytes)
    {
        try
        {
            if (sendQueue.Count > 0)
            {
                sendQueue.Enqueue(bytes);
            }
            else
            {
                socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(SendBytesCallBack), socket);
            }
        }
        catch
        {
            DebugEx.LogError("发送时发生异常");
        }
    }

    private void SendBytesCallBack(IAsyncResult vAsyncSend)
    {
        try
        {
            if (sendQueue.Count > 0)
            {
                var bytes = sendQueue.Dequeue();
                socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(SendBytesCallBack), socket);
            }
        }
        catch (Exception ex)
        {
            DebugEx.Log(ex);
        }

    }


}
