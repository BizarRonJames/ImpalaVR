using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HazardMenu : MonoBehaviour
{

    private Hazard _hazard;
    public GameObject WCGWMENU;
    [SerializeField] Hazard hazard;

    public Hazard Hazard { get => hazard; set => hazard = value; }

    public void SetGameLayerRecursive(GameObject _go, int _layer)
    {
       // _go.tag = "Untagged";
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);

        }
    }
    public void setHazardPriority(int p)
    {
        if(p==0)
        {
            Hazard.hazardState = "None";
            //SetGameLayerRecursive(hazard, 0);
        }
        else if(p<8)
        {
            Hazard.hazardState = "Low";
            //SetGameLayerRecursive(hazard, 21);
        }
        else if (p < 20)
        {
            Hazard.hazardState = "Medium";
        }

        else if (p <=36)
        {
            Hazard.hazardState = "High";
        }
        else
        {
            print("Invalid Hazard Priority number");
        }
        Hazard.tag = "Untagged";
        Hazard.GetComponent<Collider>().enabled = false;
        Hazard = null;


        // Display the WCGW menu
        WCGWMENU.GetComponent<Canvas>().enabled = true;
        WCGWMENU.transform.position = this.gameObject.transform.position;
        WCGWMENU.transform.rotation = this.gameObject.transform.rotation;

        this.gameObject.GetComponent<Canvas>().enabled = (false);
        this.gameObject.transform.position = new Vector3(0,0,0);

    }

}
