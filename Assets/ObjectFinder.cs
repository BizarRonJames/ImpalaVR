using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFinder : MonoBehaviour
{
    [SerializeField] private GameObject objectToFind;
    // Start is called before the first frame update
    void Start()
    {
        objectToFind = FindObjectOfType<HazardManager>().gameObject;

        if(objectToFind != null){
            Debug.Log(objectToFind.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
