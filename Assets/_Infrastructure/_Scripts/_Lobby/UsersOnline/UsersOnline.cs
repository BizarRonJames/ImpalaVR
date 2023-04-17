using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersOnline : MonoBehaviour
{
    public static UsersOnline instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of UsersOnline in this scene");
    }

    [SerializeField] Transform itemsParent;
    [SerializeField] OnlineUserItem item;
    List<OnlineUserItem> items = new List<OnlineUserItem>();

    public void ListUsers(OnlineUser[] users, InvitedUserResult[] invited)
    {
        foreach(OnlineUser user in users)
        {
            if (user.user_id != Player.instance.id)
            {
                bool flag = false;
                Debug.Log("Checking User: " + user.user_id);
                foreach (OnlineUserItem i in items)
                {
                    if (i.userID == user.user_id)
                    {
                        flag = true;
                        bool inv = false;
                        foreach (InvitedUserResult n in invited)
                        {
                            if (n.user_id == i.userID)
                                inv = true;
                        }

                        i.gameObject.SetActive(!inv);
                    }
                }
                if (!flag)
                {
                    OnlineUserItem go = GameObject.Instantiate(item.gameObject, itemsParent).GetComponent<OnlineUserItem>();
                    go.gameObject.SetActive(true);
                    go.Initialize(user.first_name, user.second_name, user.user_id);
                    items.Add(go);
                }
            }
        }

        foreach(OnlineUserItem i in items)
        {
            bool flag = false;
            foreach (OnlineUser user in users)
                if (user.user_id == i.userID)
                    flag = true;
            /*if (!true)
            {
                this.items.Remove(i);
                Destroy(i.gameObject);
            }*/
        }
    }
}
