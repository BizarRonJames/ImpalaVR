using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToEnable : MonoBehaviour
{
    public GameObject obj;

    void Start()
    {
        StartCoroutine(WaitToEnable_I());
    }

    IEnumerator WaitToEnable_I()
    {
        yield return new WaitForSeconds(5);
        obj.SetActive(true);
    }
}
