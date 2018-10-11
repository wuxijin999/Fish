using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    FishSocket socket;
    public bool connected { get { return socket != null && socket.connected; } }

    public void Connect(string ip, int port, bool force)
    {
        if (force)
        {
            if (connected)
            {
                socket.CloseConnect();
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

    }

    public void DisConnect()
    {

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
                    }
                    break;
                default:
                    break;
            }
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
