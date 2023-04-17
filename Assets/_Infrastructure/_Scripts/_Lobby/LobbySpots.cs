using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpots : MonoBehaviour
{
    public static LobbySpots instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of LobbySpots in this scerne");
    }

    public LobbySpot[] spots;
    public bool shouldJoinLobby = true;

    private void Start()
    {
        for(int i = 0; i < spots.Length; i++)
        {
            spots[i].index = i;
        }

    }

    public void TurnOffJoinLobby()
    {
        shouldJoinLobby = false;
    }

    public LobbySpot GetOpenSpot()
    {
        foreach (LobbySpot spot in spots)
            if (!spot.occupied)
                return spot;
        return null;
    }

    public LobbySpot GetOpenSpot(int i)
    {
        if (i >= 0 && i < spots.Length)
            return spots[i];
        return null;
    }
}
