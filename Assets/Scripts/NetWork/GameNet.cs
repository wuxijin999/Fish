using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameNet : SingletonMonobehaviour<GameNet>
{
    State m_NetState = State.NerverConnect;
    State netState {
        get { return m_NetState; }
        set {
            if (m_NetState != value)
            {
                states[m_NetState].Exit();
                m_NetState = value;
                states[m_NetState].Enter();
            }
        }
    }

    Dictionary<State, NetState> states = new Dictionary<State, NetState>() {
        { State.NerverConnect,new NerverConnectState() },
        { State.AccountLogin,new AccountLoginState() },
        { State.EneterWorld,new EnterWorldState() },
        { State.Connect,new ConnectState() },
        { State.Disconnect,new DisConnectState() },
    };

    Action<bool> onComplete;
    FishSocket socket;
    public bool connected { get { return socket != null && socket.connected; } }

    public void BeginConnect(string ip, int port, bool force, Action<bool> onComplete)
    {
        this.onComplete = onComplete;
        if (force)
        {
            if (connected)
            {
                socket.DisConnect();
            }

            socket = new FishSocket();
            socket.Connect(ip, port, OnConnect);
        }
        else
        {
            if (!connected)
            {
                socket = new FishSocket();
                socket.Connect(ip, port, OnConnect);
            }
        }
    }

    private void OnConnect(bool ok)
    {
        if (this.onComplete != null)
        {
            this.onComplete(ok);
            this.onComplete = null;
        }
    }

    public void DisConnect()
    {
        this.onComplete = null;
    }

    private void Update()
    {
        var state = states[m_NetState];
        state.OnUpdate();
        if (state.CanExit())
        {
            switch (m_NetState)
            {
                case State.NerverConnect:
                    netState = State.AccountLogin;
                    break;
                case State.AccountLogin:
                    if (Login.Instance.IsAccountLoginOk())
                    {
                        netState = State.EneterWorld;
                    }
                    else
                    {
                        netState = State.NerverConnect;
                    }
                    break;
                case State.EneterWorld:
                    if (Login.Instance.IsEnterWorldOk())
                    {
                        netState = State.Connect;
                    }
                    else
                    {
                        netState = State.NerverConnect;
                    }
                    break;
                case State.Connect:
                    if (IsNetWorkReachable())
                    {
                        Login.Instance.ReAccountLogin();
                        netState = State.AccountLogin;
                    }
                    else
                    {
                        netState = State.Disconnect;
                    }
                    break;
                case State.Disconnect:
                    netState = State.NerverConnect;
                    break;
                default:
                    break;
            }
        }
    }

    public bool IsNetWorkReachable()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                return false;
            default:
                return true;
        }
    }

    public enum State
    {
        NerverConnect,
        AccountLogin,
        EneterWorld,
        Connect,
        Disconnect,
    }


}
