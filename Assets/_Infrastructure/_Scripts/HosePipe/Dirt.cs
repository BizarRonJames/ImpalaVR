using SamDriver.Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dirt : MonoBehaviour
{
    

    public DecalMesh imgDecalMaxDirt;
    public DecalMesh imgDecalHalfDirt;
    public DecalMesh imgDecalQuarterDirt;

    public float modifier = 0.03f;
    public float value = 1;


    void Start(){
        // controller of the hose down use-case
        HoseDownManager.Instance.countDirt();
        
        
    }

    /**
     * <summary>
     * is called by the Spray class when it collides with a dirt object. Fades the dirt until it has disappeard 
     * </summary>
     */
    public void Hit()
    {
        // fade the dirt
        value -= modifier;
        SetImageAlpha(value);
        
       Debug.Log("Hit Object: "+ value);

        // check if the dirt should be deactivated
        if (value<=-0.75)
        {

            Debug.Log("Cleaned" + value);
            GetComponent<Collider>().enabled = false;
            // tally the newly cleaned dirt
            HoseDownManager.Instance.countCleanedDirt();
            
        }
    }


    /**
     * <summary>
     * helper function for debugging. Resets the dirt to its starting state
     * </summary>
     */
    [ContextMenu("Reset Object")]
    public void ResetObject()
    {
        // value = 1;
        // GetComponent<Collider>().enabled = true;
        // SetImageAlpha(1);
    }

    /**
     * <summary>
     * function to fade the different dirt layers
     * </summary>
     * <param name="a">
     * the new level of fade
     * </param>
     */
    void SetImageAlpha(float a)
    {
        if(a>0) imgDecalMaxDirt.Opacity = a;
        if(a>-0.5) imgDecalHalfDirt.Opacity = a+0.5f;
        if(a>-0.75) imgDecalQuarterDirt.Opacity = a+0.75f;

        // required to update the driven decals during the build's runtime
        imgDecalMaxDirt.SetupMaterialPropertyBlock();
        imgDecalHalfDirt.SetupMaterialPropertyBlock();
        imgDecalQuarterDirt.SetupMaterialPropertyBlock();
    }


}
