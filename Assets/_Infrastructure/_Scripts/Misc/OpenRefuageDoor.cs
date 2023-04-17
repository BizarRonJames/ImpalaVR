using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRefuageDoor : MonoBehaviour
{

    // the door that needs to be opened and closed
    public GameObject refugeDoor;

    // the node that is infront of the door(outside)
    public GameObject user;

    // state

    private bool isOpen;
    private Quaternion initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player");

        // store the original rotation the target object currently has
        initialRotation = refugeDoor.transform.rotation;

        if(!refugeDoor || !user){
            print("WARN! door or user not set");
        }
        
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        // find the distance between the user and the door
        float dist =(user.transform.position-refugeDoor.transform.position).magnitude;

        // check if its less than 2 meters
        if(dist<2){

            //check if the door is already open
            if(!isOpen){
               
                StartCoroutine(animateDoorSwing(true)); //open door
                isOpen = true;
            }
        }
        else{
            
            //check if the door is already closed
            if(isOpen){
                StartCoroutine(animateDoorSwing(false));  //close door
                isOpen = false;
            }
        }
    }

    /**
     * <summary>
     * animation loop function to open/close door
     * </summary>
     * <param name="mustOpen">
     *  - determines if its an open or close animation
     * </param>
     * <returns></returns>
     */
    private IEnumerator animateDoorSwing(bool mustOpen){

        // ensure the door is always look in either of its two states
        refugeDoor.transform.rotation= initialRotation;

        // open animation
        if(mustOpen){
            for(int i= 0;i<120;i++){
                refugeDoor.transform.Rotate(0,-1,0,Space.World);
                yield return new WaitForSeconds(0.005f);
            }


        }
        // close animation
        else{

            refugeDoor.transform.Rotate(0,-120,0,Space.World);
            for(int i= 0;i<120;i++){
                refugeDoor.transform.Rotate(0,1,0,Space.World);
                yield return new WaitForSeconds(0.005f);
            }
        }
        yield return null;
    }
}
