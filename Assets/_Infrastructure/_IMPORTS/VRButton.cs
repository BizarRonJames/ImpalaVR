using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{

    public UnityEvent eventPointerEntered;
    public UnityEvent eventPointerExited;

    public void PointerEntered()
    {
        if (eventPointerEntered != null)
            eventPointerEntered.Invoke();
    }

    public void PointerExited()
    {        
        if (eventPointerExited != null)
            eventPointerExited.Invoke();

    }
}
