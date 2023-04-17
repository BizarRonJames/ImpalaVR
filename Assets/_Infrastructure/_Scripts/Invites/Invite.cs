using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Invite : MonoBehaviour
{
    [SerializeField] TMP_Text lblOwner;
    [SerializeField] TMP_Text lblCreatedOn;

    public string sessionName;

    public void Initialize(string owner, string created_at, string session_name)
    {
        lblOwner.text = owner;
        lblCreatedOn.text = created_at;
        sessionName = session_name;
    }

    [ContextMenu("Join Em")]
    public void JoinSession()
    {
        MenuManager.instance.HideAllMenus();
        Invites.instance.ConnectToRoom(sessionName);        
        PlayerPrefs.SetString("sessionName", sessionName);
    }

}
