using SamDriver.Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demarcation : MonoBehaviour
{


    void Start(){
        
        DemarcationManager.Instance.countDemarcation();
    }

    /**
     * <summary>
     * is called by the SprayPaint class when it collides with a demarcation object.
     * </summary>
     */
    public void Hit()
    {
        //DemarcationManager.Instance.countSprayedDemarcation();
        //Destroy(this);
    }



}
