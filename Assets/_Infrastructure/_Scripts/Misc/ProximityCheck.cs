using Autohand;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Used to check if a user is within a certain radius of this gameObject
/// Will only check for one collison
/// </summary>
public class ProximityCheck : MonoBehaviour
{

    public UnityEvent proximityEntered = new UnityEvent();

    // the max distance that the object can be triggered in
    public float radius;

    // the user to teleport
    public AutoHandPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject == null) { throw new NullReferenceException("No gameobject attached to Proximity check"); }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AutoHandPlayer>();
        if (player == null) { throw new NullReferenceException("No player found in scene"); }
    }

    void Update()
    {
        // check if user is in radius
        if(Vector3.Distance(player.transform.position, this.gameObject.transform.position) < radius)
        {
            // send out notification event
            proximityEntered.Invoke();

            // disable this object
            Destroy(this.gameObject);
        }
    }
}
