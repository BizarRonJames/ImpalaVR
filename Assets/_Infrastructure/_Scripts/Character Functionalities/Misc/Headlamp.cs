using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headlamp : MonoBehaviour
{
    public Light headLamp;
    //State
    bool isOn;
    // Start is called before the first frame update
    void Start()
    {
       isOn = false;
       NetworkedPlayer.instance?.localHeadLampChange(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void flipLamp(){
        isOn = !isOn;
        headLamp.enabled = isOn;
        NetworkedPlayer.instance?.localHeadLampChange(isOn);
    }
}
