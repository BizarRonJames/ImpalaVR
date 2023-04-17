using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/**
 * <summary>
 * Contoller script that manages the physics of a barring rock
 * </summary>
 */
public class BarringRock : MonoBehaviour
{
    private Vector3 initalPosition;
    private Quaternion initalRotation;
    // Start is called before the first frame update

    private Rigidbody gRigidbody;
    private GameObject gObject;
    void Start()
    {
        // Check that this script is attached to a valid gameObject
        if((this.gameObject == null) ||
        (this.gameObject.GetComponent<Rigidbody>() == null)){
            Debug.Log("Barring Rock Has Invalid Gameobject");
            Destroy(this);
        }

        // assign private memebers
        gObject = this.gameObject;
        gRigidbody = this.gameObject.GetComponent<Rigidbody>();

        // save initial rotation
        initalRotation = gObject.transform.rotation;

        initalPosition = gObject.transform.position;

        // set the tag
        gObject.tag = "BarringRock";

        // set the object to convex only after its placed into the scene
        this.GetComponent<MeshCollider>().convex=true;

        // tell the barring manager that i am in the scene
        BarringManager.Instance.countRock();
    }

    /**
     * <summary>
     * Checks how far the barring rock has rotated from the intial rotation
     * Drops the rock, if it's suffeciently dislodged
     * </summary>
     */
    public void checkNewRotation(){

        // get the change of rotation and position
        Quaternion deltaRotation = initalRotation*Quaternion.Inverse( gObject.transform.rotation);
        float deltaPosition = Math.Abs((initalPosition-gameObject.transform.position).magnitude);

        // compare each component of the new rotation/position with the old and check how far it has shifted
        if( (Math.Abs(deltaRotation.x)>=0.03f)  ||
        (Math.Abs(deltaRotation.y)>=0.03f)  ||
        (Math.Abs(deltaRotation.z)>=0.03f) ){
            enableDrop();
            
            Debug.Log("Log!: x-> " + (Math.Abs(deltaRotation.x)) );
            Debug.Log("Log!: y-> " + (Math.Abs(deltaRotation.y)) );
            Debug.Log("Log!: z-> " + (Math.Abs(deltaRotation.z)) );
        }
        else if(deltaPosition >= 0.01){
            enableDrop();
            
            Debug.Log("Log!: deltaPostion " + (deltaPosition) );
        }
    }

    /**
     * <summary>
     * unfreezes the rigidbody
     * </summary>
     */
    void enableDrop(){
        gRigidbody.constraints = RigidbodyConstraints.None;
        gRigidbody.drag = 0;
        
        BarringManager.Instance.countBarredRock();
        //gRigidbody.useGravity = true;

    }


}
