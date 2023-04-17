using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineIndicator : MonoBehaviour
{
    public static OnlineIndicator instance;


    public void ClaimInstance()
    {
        instance = this;
        Online();
    }

    public GameObject pole;
    public GameObject online;
    public GameObject offline;

    private void Start()
    {
        pole.SetActive(false);
    }

    public void Online()
    {
        pole.SetActive(true);
        offline.SetActive(false);
        online.SetActive(true);
    }

    public void Offline()
    {
        pole.SetActive(true);
        offline.SetActive(true);
        online.SetActive(false);
    }
}
