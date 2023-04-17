using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceBattery : MonoBehaviour
{

    bool started = false;
    private void Start()
    {
        
    }

    public void StartCourse()
    {
        if (!started)
        {
            if (CourseDetails.instance.courseStarted)
                return;
            SessionDetails.instance.started = true;
            if (NetworkedPlayer.instance.isHost && CourseDetails.instance.courseType != "Demonstration")
            {

                foreach (NetworkedPlayer np in PlayerIndex.instance.players)
                {
                        np.AssignToScry(np.playerIndex - 1);
                }
            }

            if (CourseDetails.instance.courseType == "Demonstration")
            {
                if (NetworkedPlayer.instance.isHost)
                    WorkingAreas.instance.areas[1].Claim();
                else
                    WorkingAreas.instance.areas[1].TeleportTo(false);
            }
            else
                WorkingAreas.instance.areas[NetworkedPlayer.instance.playerIndex].Claim();
            started = true;
        }
        
    }

    
}
