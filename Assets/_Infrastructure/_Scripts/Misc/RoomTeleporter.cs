using Autohand;
using Oculus.Platform.Samples.VrHoops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomTeleporter : MonoBehaviour
{

    public Transform teleportLocation;
    public Button button;

    public AutoHandPlayer player; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AutoHandPlayer>();
        button.onClick.AddListener(buttonClick);
    }
    void buttonClick()
    {
        player.SetPosition(teleportLocation.position);
    }
}
