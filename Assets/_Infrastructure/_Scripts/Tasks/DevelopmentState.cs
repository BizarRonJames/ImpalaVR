using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentState : MonoBehaviour
{
    public static DevelopmentState instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of DevelopmentState in this scene");
    }

    public bool IsTesting = true;
}
