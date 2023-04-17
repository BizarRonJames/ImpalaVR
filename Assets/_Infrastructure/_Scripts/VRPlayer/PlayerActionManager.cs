using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerActionState{
    none = 0,
    climbing = 1
}
public class PlayerActionManager : MonoBehaviour
{
    public PlayerActionState playerActionState = PlayerActionState.none;
    public static PlayerActionManager instance;

    private void Awake() {
        instance = this;
    }

    public event Action<Vector3> OnClimbUpEnter;
    public void ClimbUpEnter(Vector3 climbPosition){
        if(OnClimbUpEnter != null){
            playerActionState = PlayerActionState.climbing;
            Debug.Log("Climbing Up " + climbPosition);
            OnClimbUpEnter(climbPosition);
        }
    }

    public event Action<Vector3> OnClimbDownEnter;
    public void ClimbDownEnter(Vector3 climbPosition){
        if(OnClimbDownEnter != null){
            OnClimbDownEnter(climbPosition);
        }
    }


    public event Action OnClimbExit;
    public void ClimbExit(){
        if(OnClimbExit != null){
            Debug.Log("CLimb Exit");
            playerActionState = PlayerActionState.none;
            OnClimbExit();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerActionState = PlayerActionState.none;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
