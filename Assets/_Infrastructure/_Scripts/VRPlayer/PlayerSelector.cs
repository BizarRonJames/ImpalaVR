using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    enum PlayerType{
        Quest,
        Vive,
        KeyboardMouse
    }

    [SerializeField] PlayerType playeType;

    [Header("Player Objects")]
    public GameObject QuestPlayer;
    public GameObject IndexPlayer;
    public GameObject KeyboardPlayer;

    private void Awake()
    {        
        if (playeType == PlayerType.Quest)
        {
            if(IndexPlayer)
                IndexPlayer.SetActive(false);
            if(KeyboardPlayer)
                KeyboardPlayer.SetActive(false);
            QuestPlayer.SetActive(true);
            InitializePlayer(QuestPlayer.GetComponent<Player>());
        }
        else if (playeType == PlayerType.Vive)
        {
            if (KeyboardPlayer)
                KeyboardPlayer.SetActive(false);
            QuestPlayer.SetActive(false);
            if (IndexPlayer)
            {
                IndexPlayer.SetActive(true);
                InitializePlayer(IndexPlayer.GetComponent<Player>());
            }
            else
            {
                QuestPlayer.SetActive(true);
                InitializePlayer(QuestPlayer.GetComponent<Player>());
            }
        }
        else if (playeType == PlayerType.KeyboardMouse)
        {
            if (IndexPlayer)
                IndexPlayer.SetActive(false);
            QuestPlayer.SetActive(false);
            if (KeyboardPlayer)
            {
                KeyboardPlayer.SetActive(true);
                InitializePlayer(KeyboardPlayer.GetComponent<Player>());
            }
            else
            {
                QuestPlayer.SetActive(true);
                InitializePlayer(QuestPlayer.GetComponent<Player>());
            }
        }
    }

    void InitializePlayer(Player player)
    {
        StartCoroutine(WaitForPlayer(player));
    }

    IEnumerator WaitForPlayer(Player player)
    {        
        RealtimeAvatarManager rtm = FindObjectOfType<RealtimeAvatarManager>();
        while (!rtm)
        {
            rtm = FindObjectOfType<RealtimeAvatarManager>();
            yield return null;
        }
        while(!rtm.localAvatar)
            yield return null;

        while (rtm.localAvatar.localPlayer == null)
            yield return null;

        rtm.localAvatar.localPlayer.root = player.transform;
        rtm.localAvatar.localPlayer.head = player.head.transform;
        rtm.localAvatar.localPlayer.leftHand = player.l_Hand.transform;
        rtm.localAvatar.localPlayer.rightHand = player.r_Hand.transform;
    }
}
