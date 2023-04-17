using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Lobby : MonoBehaviour
{
    public static Lobby instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of Lobby in this scene");
    }


    OnlineUsers onlineUsers;
    IEnumerator checkForUsers_I;

    public void StartLobby()
    {        
        checkForUsers_I = CheckForUsers_I();
        StartCoroutine(checkForUsers_I);
    }

    public void StopCheckForOnlineUsers()
    {
        if (checkForUsers_I != null)
            StopCoroutine(checkForUsers_I);
    }

    IEnumerator CheckForUsers_I()
    {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (true)
        {
            yield return Api.instance.GetOnlineUsers(Player.instance.company_id, RecievedUsers, NotRecievedUsers);
            yield return wait;
        }
    }

    void RecievedUsers(string info)
    {       
        try
        {
            onlineUsers = JsonUtility.FromJson<OnlineUsers>(info);
            //Debug.Log("Total Invited: " + onlineUsers.invited.Length);
            foreach (InvitedUserResult i in onlineUsers.invited)
                Debug.Log("Invited: " + i.user_id);
            UsersOnline.instance.ListUsers(onlineUsers.users, onlineUsers.invited);            
        }
        catch(Exception ex) 
        {
            Debug.LogError(ex);
        }
    }

    void NotRecievedUsers(string info)
    {
        Debug.Log("An error occurred while retrieving online users");
    }


}

[System.Serializable]
public class OnlineUsers
{
    public OnlineUser[] users;
    public InvitedUserResult[] invited;
}

[System.Serializable]
public class OnlineUser
{
    public int user_id;
    public string first_name;
    public string second_name;
}

[System.Serializable]
public class InvitedUserResult
{
    public int user_id;
}
