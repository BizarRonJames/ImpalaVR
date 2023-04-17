using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIndex : MonoBehaviour
{
    public static PlayerIndex instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of PlayerIndex in this scene");
    }

    public List<NetworkedPlayer> players = new List<NetworkedPlayer>();

    public void AddPlayer(NetworkedPlayer player)
    {
        if (!players.Contains(player))
            players.Add(player);
            return;
        
            
        StartCoroutine(WaitForNetworkedPlayer(player));        
    }

    IEnumerator WaitForNetworkedPlayer(NetworkedPlayer player)
    {
        while (!NetworkedPlayer.instance)
            yield return null;

        if (NetworkedPlayer.instance.isHost)
            NetworkedPlayer.instance.SetPlayerIndex(0);
        else
        {
            int index = 1;
            for(int i = 1; i < players.Count; i++)
            {
                if (players[i] != NetworkedPlayer.instance && players[i].id < NetworkedPlayer.instance.id  && !players[i].isHost)
                    index++;
            }

            if(Player.instance.isHost)
                NetworkedPlayer.instance.SetPlayerIndex(0);
            else
                NetworkedPlayer.instance.SetPlayerIndex(index);
        }
        /*

        List<NetworkedPlayer> newList = players;

        for(int i=0; i<newList.Count-1; i++){
            if(newList[i].isHost && i!=0){
                NetworkedPlayer np = newList[0];
                newList[0]=newList[i];
                newList[i]=np;
            }
        }

        for(int i=1; i<newList.Count-1; i++)
            for(int j = i+1;j<newList.Count;j++)
                if(newList[i].id<newList[j].id){
                    NetworkedPlayer np = newList[i];
                    newList[i] = newList[j];
                    newList[j] = np;
                    newList[i].playerIndex = i;
                    newList[j].playerIndex = j;
                }
        
        Player.instance.playerIndex = NetworkedPlayer.instance.playerIndex;
        SessionDetails.instance.player_index = NetworkedPlayer.instance.playerIndex;
        NetworkedPlayer.instance.SetPlayerIndexS(NetworkedPlayer.instance.playerIndex);
        Debug.Log("Sorted new Player list");
        players = newList;*/

    }

    public void RemovePlayer(NetworkedPlayer player)
    {
        if (players.Contains(player))
            players.Remove(player);
    }


}
