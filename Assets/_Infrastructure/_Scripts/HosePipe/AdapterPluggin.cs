using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdapterPluggin : MonoBehaviour
{

    // the gameobject this collider is attached to
    public GameObject body;
    public GameObject nozzle;


    void Start(){
        // check if a body has been defined for this trigger collider
        if( body==null ){
            if(this.transform.parent != null){
                body = this.transform.parent.gameObject ;
            }
            else{
                print("Error! no body has been attached adapter collider");
            }
        }

        // check if a nozzle has been defined
        if( nozzle==null ){
            //find the nozzle with its take
            nozzle = GameObject.FindGameObjectWithTag("HoseNozzle");
            if(this.transform.parent != null){
                body = this.transform.parent.gameObject ;
            }
            else{
                print("Error! no nozzle has been attached adapter collider");
            }
        }
    }

    /**
     * <summary>
     * attaches the Adapter of the hose to the manifold
     * </summary>
     * <param name="attachee">
     * the object the adapter is attached to
     * </param>
     */

    public void Attach(GameObject attachee){
        HoseDownManager.Instance.AdapterAttached.Invoke();

        nozzle.GetComponent<Spray>().setAttachedState(true);
        body.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        body.transform.SetParent(attachee.transform);
        body.gameObject.transform.SetPositionAndRotation(attachee.transform.position,attachee.transform.rotation);
        
    }

    /**
     * <summary>
     * breaks the attachement from the minifold
     * </summary>
     */

    [ContextMenu("Detach")]
    public void Detach(){
        nozzle.GetComponent<Spray>().setAttachedState(false);
        body.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        body.transform.parent = null;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(enableTriggerCollisions());
    }

    IEnumerator enableTriggerCollisions()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        yield return null;
    }

    /**
     * <summary>
     * check if the adapter collides with the manifold attachment
     * </summary>
     * <param name="collision"> the colliosn gameobject</param>
     */
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "ManifoldAttachment")
        {

            Debug.Log("Log! Adapter has hit the manifold");
            Attach(collision.gameObject);
        }
    }
}

