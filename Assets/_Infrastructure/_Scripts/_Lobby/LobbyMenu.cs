using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class LobbyMenu : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Menu>().Add_PostShown(CreateSession);
    }

    public void CreateSession()
    {
        if (Player.instance.IsAdmin())
            Api.instance.CreateSession(SessionDetails.instance.SessionCreated, SessionDetails.instance.SessionCreationFailed, Player.instance.id);
        
        else
            Debug.LogError("You do not have the required authority to host a session");
    }
   
}

[System.Serializable]
public class SessionResults
{
    public string status;
    public SessionData data;
}

[System.Serializable]
public class SessionData
{
    public int id;
    public string created_at;
    public string session_name;
    public string session_key;
    public int host_id;
    public int course_id;
}
