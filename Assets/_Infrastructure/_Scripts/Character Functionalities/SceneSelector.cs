using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Autohand{
public class SceneSelector : MonoBehaviour
{
    public HandCanvasPointer SceneMenu;
    // Singleton
    private SceneSelector Instance;

    public OVRScreenFade Fader;

    public AutoHandPlayer ahp;

    // Buttons from the scene selector canvas
    public Button A;
    public Button B;
    public Button C;
    // the active scene
    private int activeScene;

    // Unity Controls
    [ContextMenu("A Button")]
    private void Abutton(){
        StartCoroutine(setScene(0));
    }
    [ContextMenu("B Button")]
    private void Bbutton(){
        StartCoroutine(setScene(1));
    }
    [ContextMenu("C Button")]
    private void Cbutton(){
        StartCoroutine(setScene(2));
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //singleton check
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
            activeScene = -1;

            if(A && B && C){
                A.onClick.AddListener(delegate{ StartCoroutine(setScene(0)); });
                B.onClick.AddListener(delegate{ StartCoroutine(setScene(1)) ; });
                C.onClick.AddListener(delegate{ StartCoroutine(setScene(2)); });
            }
        } 
    }

    public IEnumerator setScene(int ss){
        // check if the scene is changing
        if(activeScene!=ss){
            activeScene = ss;

            //checked if scene is int range of available scenes
            if(ss < 3 && ss >=0){

                string Scene;

                // switch
                if(ss==0){
                    Scene = "Pre_production_Mesh bakes";
                }else if(ss==1){
                    Scene = "Pre-Production_Refuge Chamber";
                }else {
                    Scene = "Pre-Production_Waiting Area";
                }


                // change scene
                yield return new WaitForFixedUpdate();
                yield return Fader.Fade(0f,1f);
                yield return LoadSceneAsync(Scene ) ;
                yield return null;
            }
        }

        IEnumerator LoadSceneAsync(string s){

            activeScene = ss;
            ahp.useGrounding=false;
            ahp.gameObject.GetComponent<Rigidbody>().useGravity = false;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(s); // change "YourSceneName" with the scene you want to load

            // wait until the scene fully loads
            while (!asyncLoad.isDone)
            {
                
                yield return null;
            }

            SceneMenu.gameObject.SetActive(true);
        }   

    }

}
}