
using UnityEngine;
using UnityEngine.Sprites;
using System.Collections;

public class LocationNode : MonoBehaviour {



    // sprite for when the marker is hovered upon 
    public SpriteRenderer openSprite;
    // default sprite state
    public SpriteRenderer closedSprite;

    // sprites size
    private Vector3 scaleLock ;
    // state
    private bool isOpen;
    private float scale;

    public void Start(){

        // ensuring a deep copy of the objects scale vector
        // this will be used as a reference when we grow/shrink the circle
        Vector3 tmp = openSprite.transform.localScale;
        scaleLock = new Vector3(tmp.x,tmp.y,tmp.z);
 
        openSprite.enabled = false;
        openSprite.transform.localScale = new Vector3(0,0,0);

        // check if the sprites have been set
        if(!openSprite || !closedSprite){
            Destroy(this);
        }
        isOpen = true;
        close();

        //print("node sprite initialized");
        
    }

    /**
     * <summary>
     * plays the open animation and changes the state of the node to open
     * </summary>
     */
    public void open(){
        if(!isOpen){
            StartCoroutine(animate(true));
            isOpen=true;
        }
    }

    /**
     * <summary>
     * plays the close animation and changes the state of the node to closed
     * </summary>
     */
    public void close(){ 
        if(isOpen){
            StartCoroutine(animate(false));
            isOpen=false;
        }
    }

    /**
     * <summary>
     * animation loop function to grow/shrink circle
     * </summary>
     * <param name="mustOpen">
     *  - determines if its an open or close animation
     * </param>
     * <returns></returns>
     */
    private IEnumerator animate(bool mustOpen){

        // open animation
        if(mustOpen){
            
            openSprite.transform.localScale = new Vector3(0,0,0);

            // display openSprite and wait
            openSprite.enabled = true;
            yield return new WaitForSeconds(0.02f);
            
            // hide closeSprite and wait
            closedSprite.enabled = false;
            yield return new WaitForSeconds(0.02f);

            // shrink the openCircle, loop and wait
            for(int i =0;i<10;i++){
                openSprite.transform.localScale += scaleLock/10;
                yield return new WaitForSeconds(0.03f);
            }


        }
        // close animation
        else{

            openSprite.transform.localScale = new Vector3(scaleLock.x,scaleLock.y,scaleLock.z);

            //inverse of the open animation
            for(int i =0;i<10;i++){
                scale-=0.1f;
                
                openSprite.transform.localScale -= scaleLock/10;
                
                yield return new WaitForSeconds(0.02f);
            }


            // wait then display closedSprite 
            yield return new WaitForSeconds(0.02f);
            closedSprite.enabled = true;
            
            // wait then hide openSprite 
            yield return new WaitForSeconds(0.03f);
            openSprite.enabled = false;
        }
        yield return null;
    }
}