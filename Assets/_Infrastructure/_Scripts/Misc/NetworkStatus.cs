using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkStatus : MonoBehaviour
{
    public static NetworkStatus instance;
    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of NetworkStatus in this scene");
    }

    bool connected = false;

    public bool IsConnected
    {
        get { return connected; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Realtime realTime = FindObjectOfType<Realtime>();
        realTime.didConnectToRoom += Connected;
        realTime.didDisconnectFromRoom += Disconnected;
    }

    private void Connected(Realtime realtime)
    {
        connected = true;
    }

    private void Disconnected(Realtime realtime)
    {
        connected = false;
    }
}
