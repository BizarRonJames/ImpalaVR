using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Autohand{
    public class LadderObserver : MonoBehaviour
    {
        // used by the task manager
        [SerializeField] public UnityEvent LadderClimbed = new UnityEvent();

        // number of hands on the ladder
        private int activeGrabs;
        // is the user crouching
        private bool crouched;

        // the user to teleport
        public AutoHandPlayer player;

        // the postion on the panel to teleport to
        public Transform upLoc;

        // wait buffer
        private float wait;

        /**
        * <summary>
        * - initialize state
        * </summary>
        */
        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<AutoHandPlayer>();
            wait = 0.0f;
            activeGrabs = 0;
            crouched = false;
        }

        /**
         * <summary>
         * check for crocuh if the user is grabbing the 
         * </summary>
         */
        void Update()
        {
            
            if(activeGrabs>=1){
                wait += Time.deltaTime;
                // wait a second and then check state again
                if(wait>1){
                    stateCheck();
                    wait = 0.0f;
                }
            }
        }
        /**
        * <summary>
        * - function to call when a grab event happens 
        * </summary>
        */
        public void addGrab(){
            activeGrabs++;
        }
        /**
        * <summary>
        * - function to call when a release event happens
        * </summary>
        */
        public void removeGrab(){
            activeGrabs--;
            wait = 0.0f;
        }
        /**
        * <summary>
        * - function to check if users height will fit into the panel
        * - check state of the ladder
        * </summary>
        */
        public void setCrouch(){
            if(player){
                if (player.headCamera.transform.position.y<this.gameObject.transform.position.y+0.6){
                    crouched=true;
                }
                else{
                    crouched = false;
                }
            }
            
        }

        /**
        * <summary>
        * - check the state of the ladder and initiate the teleport event if needed
        * </summary>
        */
        private void stateCheck(){
            //check for crouch
            setCrouch();
            if( activeGrabs>=1){
                //initiate teleport
                StartCoroutine(fadeAnimation());
            }
        }

        /**
         * <summary>
         * fade animation that teleports the user infront of the ladder
         * </summary>
         */
        private IEnumerator fadeAnimation(){
            yield return player.headCamera.GetComponent<OVRScreenFade>().Fade(0f,1f);
            yield return new WaitForSeconds(0.35f);
            Vector3 toPos = new Vector3(upLoc.position.x,upLoc.position.y,upLoc.position.z);

            // teleport user
            player.SetPosition(toPos);

            // reveal the panel nodes
            LocationNodeManager.Instance.showPanelNodes();

            yield return new WaitForSeconds(0.35f);
            yield return player.headCamera.GetComponent<OVRScreenFade>().Fade(1f,0f);

            // tell the task manager we have teleported
            LadderClimbed.Invoke();
        }
    }

}
