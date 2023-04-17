using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/**
 * <summary>
 * this script is to be applied to the head of a user.
 * it is used to determine if the user's head is in the ceiling
 * </summary>
 */
public class OutOfBoundsDetection : MonoBehaviour
{
    // an immage on a canvas that is a transparent red
    public GameObject userOverlay;

    // user's head enters the ceiling
    void OnTriggerEnter(Collider other)
    {   
        print("IN");
        userOverlay.SetActive(true);
      
    }

    /**
     * <summary>
     * - this function is used to get the warning overlay gameobject
     * </summary>
     */
    public void Start(){
        userOverlay = GameObject.FindGameObjectWithTag("Warning");
    
        userOverlay.SetActive(false);
    }
    // user's head exits the ceiling
    void OnTriggerExit(Collider other)
    {
        print("OUT");
        userOverlay.SetActive(false);
    }

}
