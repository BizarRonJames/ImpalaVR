using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OfflineMode : MonoBehaviour
{
    Realtime rt;
    public UnityEvent onConnect;

    [ContextMenu("Start Offline")]
    public void GoOffline()
    {
        StartCoroutine(WaitToConnect());
    }

    IEnumerator WaitToConnect()
    {
        while (!rt)
        {
            Debug.Log("Looking for RT");
            rt = FindObjectOfType<Realtime>();
            yield return null;
        }
        
        Debug.Log("Found for RT");
        rt.didConnectToRoom += SessionConnected;
        string sessionName = "Offline_Room_" + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss");
        PlayerPrefs.SetString("sessionName", sessionName);
        SessionDetails.instance.session_name = sessionName;
        SessionDetails.instance.player_index = 1;
        //Re enable to use offline in online
        /*rt.Connect(sessionName);
        SessionDetails.instance.started = true;*/

        //Enable to use offline as offline
        //GrabRequest[] grs = GetComponentsInChildren<GrabRequest>();
        //for(int i = 0; i < grs.Length; i++)
        //{
        //    RealtimeTransform rt = grs[i].GetComponent<RealtimeTransform>();
        //    Destroy(grs[i]);
        //    Destroy(rt);
        //}


        RealtimeTransform[] rts = GetComponentsInChildren<RealtimeTransform>();
        foreach (RealtimeTransform rt in rts)
            Destroy(rt);


        CourseDetails.instance.TurnOnAlwaysTraining();
        onConnect.Invoke();
    }

    private void SessionConnected(Realtime realtime)
    {
        NetworkedPlayer[] players = FindObjectsOfType<NetworkedPlayer>();
        Debug.Log("Total Players in scene: " + players.Length);
        onConnect.Invoke();
        realtime.didConnectToRoom -= SessionConnected;
        CourseDetails.instance.TurnOnAlwaysTraining();

    }
}
