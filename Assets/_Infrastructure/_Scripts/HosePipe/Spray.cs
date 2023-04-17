using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{

    public float vel = 0.01f;
    public int killAfter = 500;
    public float constGrav = 0.02f;
    public ParticleSystem waterAnimation;
    public bool isSpraying = false;

    private bool isAttached;

    IEnumerator spray;

    void Start(){
        isAttached = false;
        isSpraying = false;
        waterAnimation.gameObject.SetActive(false) ;
    }
    
    /**
     * <summary>
     * set the attachment state of the hosepipe. If it's true then the hose pipe will become sprayable
     * </summary>
     * <param name="state"> 
     * on / off
     * </param>
     */
    public void setAttachedState(bool state){
        isAttached = state;

        // turn off the hose if the adapter is detached
        if(state==false) StopSpray();
    }
    /**
     * <summary>
     * Turns on the hose pipe
     * </summary>
     */
    [ContextMenu("Start Spray")]
    public void StartSpray()
    {
        //check that the hose is connected to and manifold
        if (isAttached)
        {

            waterAnimation.gameObject.SetActive(true);
            isSpraying = true;
            if (spray != null)
                StopCoroutine(spray);
            spray = Spray_I();
            StartCoroutine(spray);

            waterAnimation.Play(true);


        }
        else StopSpray();
    }

    /**
     * <summary>
     * Turns off the hose pipe
     * </summary>
     */
    [ContextMenu("Stop Spray")]
    public void StopSpray()
    {
        isSpraying = false;
        if (spray != null)
            StopCoroutine(spray);
        SprayReleaseTrigger();
        waterAnimation.Stop(true, ParticleSystemStopBehavior.StopEmitting);
       // waterAnimation.gameObject.SetActive(false) ;  
    }


    public void SprayReleaseTrigger()
    {

    }
    public void SprayPullTrigger()
    {

    }
    /**
     * <summary>
     * Animation and raycast Coroutine
     * </summary>
     */
    IEnumerator Spray_I()
    {

        Debug.Log("Starting Spray");
        while (isSpraying && isAttached)
        {
            GravCast(transform.position, transform.TransformDirection(Vector3.forward)*0.1f, killAfter,vel);

            yield return null;
        }
    }
    /**
    * <summary>
    * simulates a curved raycast. shots a short raycast, if no collison, shoots another and changes its direction based on gravity
    * </summary>
    * <param name="startPos"> the intial postion </param>
    * <param name="direction"> the intial direction </param>
    * <param name="killAfter"> how long to shoot the raycast </param>
    * <param name="velocity"> the speed of each raycast segment</param>
    * <returns></returns>
    */
    Vector3[] GravCast(Vector3 startPos, Vector3 direction, int killAfter, float velocity)
    {
        RaycastHit hit;
        Vector3[] vectors = new Vector3[killAfter];
        Ray ray = new Ray(startPos, direction*velocity);
        for (int i = 0; i < killAfter; i++)
        {   
            // Turn on gizmos to render the ray
            Debug.DrawRay(ray.origin, ray.direction*velocity, Color.blue);
            ray = new Ray(ray.origin + ray.direction*velocity, ray.direction +  velocity*(Physics.gravity   * (constGrav )));
            vectors[i] = ray.origin;

            if(Physics.Raycast(ray,out hit,1f))
            {
                if (hit.collider.gameObject.GetComponent<Dirt>()){
                    Debug.Log("Hit Object: " + hit.collider.gameObject.name);
                    hit.collider.GetComponent<Dirt>().Hit();
                    return vectors;
                }
            }


        }
        return vectors;
    }
 
}
