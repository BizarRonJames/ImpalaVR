using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autohand
{
    

public class LocationNodeManager : MonoBehaviour
{
    // singleton
    public static LocationNodeManager Instance;

    // the node that has is currently in the open state
    public LocationNode activeNode;

    // these are the parent empty objects that hold the location nodes
    public GameObject panelNodes;   // can be null

    // the node of the most recent teleport
    public LocationNode lastTeleport;

    void Start()
    {
         //singleton check
        if (Instance != null && Instance != this) 
        { 
            print("WARN!: duplicate Location Manager Node Deteced");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;    
            hidePanelNodes();
        } 
    }

     /**
      * <summary>
      * sets the active node and closes the last active node
      * </summary>
      * <param name="active"> the newly selected node</param>
      */
    public void updateActive(LocationNode active){

        if (this.activeNode != active){
            // close previous node 
            if(activeNode){
                activeNode.close();
            }
            
            activeNode = active;
            // open new node
            activeNode.open();

        }

    }

    /**
     * <summary>
     * the user changes the state of a node to closed through this function
     * </summary>
     */
    public void closeActive(){
        if(activeNode){
            activeNode.close();
            activeNode=null;
        }
    }

    /**
     * <summary>
     * onSwitch - to allow the user, access to the panel nodes
     * </summary>
     */
    public void showPanelNodes(){
        // if gullNodes is set, deactivate it
        if(panelNodes){
            panelNodes.SetActive(true);
        }
    }
    /**
     * <summary>
     * offSwitch - to remove from the user, access to the panel nodes
     * </summary>
     */
    public void hidePanelNodes(){
        if(panelNodes){
            panelNodes.SetActive(false);
        }
    }
    
    /**
     * <summary>
     * - used to determine if the user is in the gulley or panel
     * - hides and shows the location nodes accordingly
     * </summary>
     */
    public void checkForPanel(){
        // ensure there is a last teleport
        if(lastTeleport){
            // we teleported onto the panel
            if(lastTeleport.transform.parent.gameObject == panelNodes ){
                showPanelNodes();
            }
            // we teleported into the gulley
            else{
                hidePanelNodes();
            }
        }
    }
    
    /**
    * <summary>
    * - sets the lastTeleport node and calls the checkForPanel Function
    * </summary>
    * <param name="last"> the node we teleported to</param>
    */
    public void updateLastTeleport(LocationNode last){
        this.lastTeleport = last;
        checkForPanel();
    }

}
}