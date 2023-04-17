using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    private void OnMouseEnter()
    {
        Debug.Log("MouseOver");
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
    }
}
