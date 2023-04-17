using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex90hz : MonoBehaviour
{
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        OVRManager.display.displayFrequency = 90f;
        Debug.Log("Upped framerate to 90");
    }
}
