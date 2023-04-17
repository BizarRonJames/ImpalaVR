using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCGW : MonoBehaviour
{
    public void hideME()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }
    
}
