using Autohand;
using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestInterface : MonoBehaviour
{
    public bool customConnectEvent = false;
    public UnityEvent onConnect;
    public UnityEvent justVisiting;
    public Button btnOffline;
    public Button btnOnline;

    public Button btnRefugeBay;


    public Button btnMine;
    Realtime rt;

    string sessionName;

    private void Start()
    {
        if (customConnectEvent)
        {
            StartCoroutine(WaitToConnect());

        }
    }

    IEnumerator WaitToConnect()
    {
        while (!rt)
        {
            rt = FindObjectOfType<Realtime>();
            yield return null;
        }
        LobbySpots.instance.TurnOffJoinLobby();
        CourseDetails.instance.isAlwaysTraining = true;
        rt.didConnectToRoom += SessionConnected;
        string sessionName = "Offline_Room_" + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm");
        SessionDetails.instance.session_name = sessionName;

        rt.Connect(sessionName);
    }

    private void SessionConnected(Realtime realtime)
    {
        NetworkedPlayer[] players = FindObjectsOfType<NetworkedPlayer>();
        Debug.Log("Total Players in scene: " + players.Length);
        StartCoroutine(WaitToConnect_I(players.Length));
        realtime.didConnectToRoom -= SessionConnected;

    }

    IEnumerator WaitToConnect_I(int total)
    {
        yield return new WaitForSeconds(1);
        if (onConnect != null && total == 1)
            onConnect.Invoke();
        else
            justVisiting.Invoke();
    }

    [ContextMenu("1. LoginA")]
    void LogInA()
    {
        FindObjectOfType<Login>().LogIn();
    }

    [ContextMenu("1. LoginB")]
    void LogInB()
    {
        FindObjectOfType<Login>().LogInB();
    }

    [ContextMenu("Go Offline")]
    public void TestOffline()
    {
        btnOffline.onClick.Invoke();
    }

    [ContextMenu("2. Start Lobby")]
    void Lobby()
    {
        FindObjectOfType<MainMenu>().ShowMenu_Lobby();
    }

    [ContextMenu("3. Start Session")]
    void StartSession()
    {
        btnOnline.onClick.Invoke();
    }

    [ContextMenu("4. Invite user")]
    void InviteUser()
    {
        OnlineUserItem[] items = FindObjectsOfType<OnlineUserItem>();
        foreach (OnlineUserItem i in items)
        {
            if (i.gameObject.activeSelf)
                i.Invite();
        }
    }

    [ContextMenu("5. Accept Invite")]
    void AcceptInvite()
    {
        Invite[] invites = FindObjectsOfType<Invite>();
        foreach (Invite i in invites)
            if (i.gameObject.activeSelf)
            {
                i.JoinSession();
                break;
            }
    }


    [ContextMenu("6. To refuge bay")]
    void toRefugeBay()
    {
        
        btnRefugeBay.onClick.Invoke();
    }

    [ContextMenu("7. To mine")]
    void toMine()
    {
        btnMine.onClick.Invoke();
    }


    public Transform toLoc;

    [ContextMenu("Teleport to the toLoc position")]
    public void TeleportToLoc()
    {
        AutoHandPlayer.Instance.SetPosition(toLoc.position);
    }
}


