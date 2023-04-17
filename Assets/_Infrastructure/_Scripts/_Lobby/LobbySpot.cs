using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpot : RealtimeComponent<LobbySpotModel>
{
    private NetworkedPlayer player;
    public bool occupied = false;
    public int index = -1;

    protected override void OnRealtimeModelReplaced(LobbySpotModel previousModel, LobbySpotModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.occupiedDidChange -= Occupied;
        }

        if(currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.occupied = false;
            }
            currentModel.occupiedDidChange += Occupied;
        }
    }

    private void Occupied(LobbySpotModel model, bool value)
    {
        occupied = value;
    }

    public void TakeSpot(NetworkedPlayer player)
    {
        this.player = player;
        if (player.GetComponent<RealtimeAvatar>().isOwnedLocallyInHierarchy)
        {
            //Player.instance.transform.position = transform.position;
            //Player.instance.transform.rotation = transform.rotation;
            Debug.Log("teleporting to lobby spot: " + gameObject.name);
            Player.instance.Teleport(transform);
            model.occupied = true;
        }        
    }
}
