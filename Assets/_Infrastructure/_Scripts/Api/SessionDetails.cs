using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDetails : MonoBehaviour
{
    public static SessionDetails instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of SessionDetails in this scene");
    }

    public int session_id;
    public string created_at;
    public string session_name;
    public string session_key;
    public int course_id;
    public int player_index = -1;
    public bool started = false;

    bool online = true;

    public void StartOffline()
    {
        online = false;
        Api.instance.CreateSession(SessionDetails.instance.SessionCreated, SessionDetails.instance.SessionCreationFailed, Player.instance.id);
        CourseDetails.instance.SetOffline();
    }

    public void SessionCreated(string json)
    {
        //Debug.Log(json);
        SessionResults results = new SessionResults();
        bool success = false;
        try
        {
            results = JsonUtility.FromJson<SessionResults>(json);

            SessionDetails.instance.session_id = results.data.id;
            SessionDetails.instance.created_at = results.data.created_at;
            SessionDetails.instance.session_name = results.data.session_name;
            SessionDetails.instance.session_key = results.data.session_key;
            SessionDetails.instance.course_id = results.data.course_id;

            success = results.status == "OK";
            if (success && SessionDetails.instance.session_name != "")
            {
                if (online)
                {
                    HostSession(SessionDetails.instance.session_name);
                    Lobby.instance.StartLobby();
                }
            }
            else
                SessionCreationFailed(json);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Could not parse results: " + ex.ToString());
        }

        if (success)
        {
            HostSession(SessionDetails.instance.session_name);
        }
        else
            SessionCreationFailed(json);
    }

    public void SessionCreationFailed(string json)
    {
        Debug.Log("Session Creation Failed");
        Debug.Log(json);
    }

    void HostSession(string sessionName)
    {
        Realtime realtime = FindObjectOfType<Realtime>();
        if (realtime)
        {
            Player.instance.isHost = true;
            PlayerPrefs.SetString("sessionName", sessionName);
            realtime.Connect(sessionName);
        }
        else
            Debug.LogError("No Realtime found in this scene");
    }
}
