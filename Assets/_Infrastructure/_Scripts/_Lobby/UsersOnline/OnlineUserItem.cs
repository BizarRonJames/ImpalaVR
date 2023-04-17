using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnlineUserItem : MonoBehaviour
{    
    [SerializeField] GameObject loadingItem;
    [SerializeField] TMP_Text lblName;

    public string _name;
    public string surname;
    public int userID;

    bool inviting = false;

    public void Initialize(string name, string surname, int userID)
    {
        this._name = name;
        this.surname = surname;
        this.userID = userID;

        lblName.text = name + " " + surname;
    }

    [ContextMenu("Invitie User")]
    public void Invite()
    {
        if (inviting)
            return;
        inviting = true;
        lblName.gameObject.SetActive(false);
        loadingItem.SetActive(true);
        Api.instance.InviteUser(userID, UserInvited, UserNotInvited);
    }

    void UserInvited(string info)
    {
        gameObject.SetActive(false);
    }

    void UserNotInvited(string info)
    {
        inviting = false;
        loadingItem.SetActive(false);
        lblName.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        lblName.gameObject.SetActive(true);
        loadingItem.SetActive(false);
        inviting = false;
    }
}
