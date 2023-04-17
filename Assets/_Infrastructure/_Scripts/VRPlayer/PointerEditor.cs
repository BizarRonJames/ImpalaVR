using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Autohand;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PointerEditor
{
    //[MenuItem("Tools/FixPointer")]
    public static void FixPointer()
    {
        AutoInputModule input = GameObject.FindObjectOfType<AutoInputModule>();
        if (input)
        {
            GameObject go = input.gameObject;
            GameObject.DestroyImmediate(input);

            EventSystem eSystem = go.GetComponent<EventSystem>();
            if (eSystem)
                GameObject.DestroyImmediate(eSystem);

            
            go.AddComponent<AutoInputModule>();
        }
        else
        {
            GameObject go = GameObject.FindObjectsOfType<Transform>()[0].gameObject;
            go.AddComponent<AutoInputModule>();
        }
    }
}


