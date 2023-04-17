using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invites : MonoBehaviour
{
    public static Invites instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of Invites in this scene");
    }

    public Invite invite;
    public Transform itemParent;
    public GameObject goLoading;

    List<Invite> invites = new List<Invite>();

    private void Start()
    {
        goLoading.SetActive(false);
    }

    public void ParseInvites(string info)
    {
        try
        {
            RefreshResults rr = JsonUtility.FromJson<RefreshResults>(info);

            //Destroy invites that's not present anymore
            foreach(Invite i in invites)
            {
                bool present = false;
                foreach (invitesResults r in rr.invites)
                {
                    if (r.session_name == i.sessionName)
                        present = true;
                }
                if(!present)
                {
                    invites.Remove(i);
                    Destroy(i.gameObject);
                }
            }

            //Create new invites
            foreach (invitesResults r in rr.invites)
            {
                bool flag = false;
                foreach (Invite i in invites)
                {
                    if (i.sessionName == r.session_name)
                        flag = true;
                }
                if (!flag)
                {
                    Invite inv = Instantiate(invite.gameObject, itemParent).GetComponent<Invite>();
                    inv.Initialize(r.first_name,r.created_at,r.session_name);
                    invites.Add(inv);
                    inv.gameObject.SetActive(true);
                    return;
                }
            }
            
        }
        catch(System.Exception ex)
        {
            OVRLipSyncDebugConsole.Log("Invite could not be parsed: " + ex.ToString());
        }
    }

    public void ConnectToRoom(string sessionName)
    {
        Debug.Log("Connecting To Session: " + sessionName);        
        Realtime realTime = FindObjectOfType<Realtime>();
        if (realTime)
        {
            realTime.Connect(sessionName);
            itemParent.gameObject.SetActive(false);
            goLoading.SetActive(true);            
        }
    }
}

[System.Serializable]
public class RefreshResults
{
    public string status;
    public int user_id;
    public invitesResults[] invites;
}

[System.Serializable]
public class invitesResults
{
    public string first_name;
    public string second_name;
    public string session_name;
    public string created_at;
}
