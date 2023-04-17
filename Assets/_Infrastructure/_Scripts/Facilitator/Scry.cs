using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scry : MonoBehaviour
{
    public static Scry instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of Scry in this scene");
    }

    public ScryFace[] faces;
}
